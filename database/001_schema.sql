-- ============================================================
-- FleetControl
-- Schema inicial do núcleo operacional
-- PostgreSQL
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- EXTENSÕES
-- ------------------------------------------------------------

CREATE EXTENSION IF NOT EXISTS btree_gist;

-- PostGIS será utilizado quando o ambiente estiver configurado.
-- CREATE EXTENSION IF NOT EXISTS postgis;


-- ------------------------------------------------------------
-- EMPRESA
-- ------------------------------------------------------------

CREATE TABLE empresa (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW()
);


-- ------------------------------------------------------------
-- MINA
-- ------------------------------------------------------------

CREATE TABLE mina (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    empresa_id BIGINT NOT NULL,
    nome VARCHAR(150) NOT NULL,
    codigo VARCHAR(50),
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_mina_empresa
        FOREIGN KEY (empresa_id)
        REFERENCES empresa(id),

    CONSTRAINT uq_mina_codigo
        UNIQUE (empresa_id, codigo)
);


-- ------------------------------------------------------------
-- OPERADOR
-- ------------------------------------------------------------

CREATE TABLE operador (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    mina_id BIGINT NOT NULL,
    nome VARCHAR(150) NOT NULL,
    matricula VARCHAR(50) NOT NULL,
    cartao VARCHAR(50),
    status VARCHAR(20) NOT NULL DEFAULT 'ATIVO',
    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_operador_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT uq_operador_matricula
        UNIQUE (mina_id, matricula),

    CONSTRAINT ck_operador_status
        CHECK (status IN ('ATIVO', 'INATIVO', 'BLOQUEADO'))
);


-- ------------------------------------------------------------
-- MODELO DE EQUIPAMENTO
-- ------------------------------------------------------------

CREATE TABLE modelo_equipamento (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_modelo_equipamento_nome
        UNIQUE (nome)
);


-- ------------------------------------------------------------
-- EQUIPAMENTO
-- ------------------------------------------------------------

CREATE TABLE equipamento (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    mina_id BIGINT NOT NULL,
    modelo_id BIGINT NOT NULL,
    codigo VARCHAR(50) NOT NULL,
    nome VARCHAR(150),
    status VARCHAR(20) NOT NULL DEFAULT 'ATIVO',
    possui_balanca BOOLEAN NOT NULL DEFAULT FALSE,
    bloqueado BOOLEAN NOT NULL DEFAULT FALSE,
    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_equipamento_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT fk_equipamento_modelo
        FOREIGN KEY (modelo_id)
        REFERENCES modelo_equipamento(id),

    CONSTRAINT uq_equipamento_codigo
        UNIQUE (mina_id, codigo),

    CONSTRAINT ck_equipamento_status
        CHECK (
            status IN (
                'ATIVO',
                'INATIVO',
                'MANUTENCAO',
                'BLOQUEADO'
            )
        )
);


-- ------------------------------------------------------------
-- POLÍGONO
-- ------------------------------------------------------------

CREATE TABLE poligono (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    mina_id BIGINT NOT NULL,
    nome VARCHAR(150) NOT NULL,
    tipo VARCHAR(30) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    -- Será preenchido com PostGIS posteriormente.
    -- geometria GEOMETRY(POLYGON, 4326),

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_poligono_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT ck_poligono_tipo
        CHECK (
            tipo IN (
                'FRENTE',
                'DESTINO',
                'AREA_OPERACIONAL'
            )
        )
);


-- ------------------------------------------------------------
-- LOCAÇÃO
-- ------------------------------------------------------------

CREATE TABLE locacao (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    mina_id BIGINT NOT NULL,

    origem_poligono_id BIGINT NOT NULL,
    destino_poligono_id BIGINT NOT NULL,

    inicio TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    fim TIMESTAMPTZ,

    status VARCHAR(20) NOT NULL DEFAULT 'ATIVA',

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_locacao_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT fk_locacao_origem
        FOREIGN KEY (origem_poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT fk_locacao_destino
        FOREIGN KEY (destino_poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT ck_locacao_periodo
        CHECK (fim IS NULL OR fim > inicio),

    CONSTRAINT ck_locacao_status
        CHECK (
            status IN (
                'ATIVA',
                'ENCERRADA',
                'CANCELADA'
            )
        )
);


-- ------------------------------------------------------------
-- ALOCAÇÃO DE EQUIPAMENTO
-- ------------------------------------------------------------
--
-- Define qual equipamento pertence à locação e qual é sua função.
--
-- TRANSPORTE = caminhão
-- CARGA      = escavadeira/carregadeira
-- APOIO      = equipamento auxiliar
--
-- O histórico é temporal.
-- ------------------------------------------------------------

CREATE TABLE alocacao_equipamento (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    locacao_id BIGINT NOT NULL,
    equipamento_id BIGINT NOT NULL,

    funcao VARCHAR(20) NOT NULL,

    inicio TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    fim TIMESTAMPTZ,

    motivo_saida TEXT,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_alocacao_equipamento_locacao
        FOREIGN KEY (locacao_id)
        REFERENCES locacao(id),

    CONSTRAINT fk_alocacao_equipamento_equipamento
        FOREIGN KEY (equipamento_id)
        REFERENCES equipamento(id),

    CONSTRAINT ck_alocacao_equipamento_funcao
        CHECK (
            funcao IN (
                'TRANSPORTE',
                'CARGA',
                'APOIO'
            )
        ),

    CONSTRAINT ck_alocacao_equipamento_periodo
        CHECK (fim IS NULL OR fim > inicio)
);


-- ------------------------------------------------------------
-- REGRA:
-- um equipamento não pode estar em duas alocações simultâneas.
-- ------------------------------------------------------------

ALTER TABLE alocacao_equipamento
ADD CONSTRAINT ex_alocacao_equipamento_periodo
EXCLUDE USING gist (
    equipamento_id WITH =,
    tstzrange(
        inicio,
        COALESCE(fim, 'infinity'::timestamptz),
        '[)'
    ) WITH &&
);


-- ------------------------------------------------------------
-- ALOCAÇÃO DE OPERADOR
-- ------------------------------------------------------------
--
-- Mantém o histórico de quem estava operando qual equipamento.
-- ------------------------------------------------------------

CREATE TABLE alocacao_operador (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    operador_id BIGINT NOT NULL,
    equipamento_id BIGINT NOT NULL,

    inicio TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    fim TIMESTAMPTZ,

    motivo_saida TEXT,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_alocacao_operador_operador
        FOREIGN KEY (operador_id)
        REFERENCES operador(id),

    CONSTRAINT fk_alocacao_operador_equipamento
        FOREIGN KEY (equipamento_id)
        REFERENCES equipamento(id),

    CONSTRAINT ck_alocacao_operador_periodo
        CHECK (fim IS NULL OR fim > inicio)
);


-- ------------------------------------------------------------
-- REGRA:
-- um operador não pode operar dois equipamentos simultaneamente.
-- ------------------------------------------------------------

ALTER TABLE alocacao_operador
ADD CONSTRAINT ex_alocacao_operador_periodo
EXCLUDE USING gist (
    operador_id WITH =,
    tstzrange(
        inicio,
        COALESCE(fim, 'infinity'::timestamptz),
        '[)'
    ) WITH &&
);


-- ------------------------------------------------------------
-- VIAGEM
-- ------------------------------------------------------------

CREATE TABLE viagem (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    codigo UUID NOT NULL DEFAULT gen_random_uuid(),

    locacao_id BIGINT NOT NULL,
    alocacao_equipamento_id BIGINT NOT NULL,

    origem_poligono_id BIGINT NOT NULL,
    destino_poligono_id BIGINT NOT NULL,

    -- Equipamento que realizou o carregamento.
    equipamento_carga_id BIGINT,

    massa NUMERIC(12,2),

    inicio TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    fim TIMESTAMPTZ,

    status VARCHAR(30) NOT NULL DEFAULT 'EM_ANDAMENTO',

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_viagem_locacao
        FOREIGN KEY (locacao_id)
        REFERENCES locacao(id),

    CONSTRAINT fk_viagem_alocacao_equipamento
        FOREIGN KEY (alocacao_equipamento_id)
        REFERENCES alocacao_equipamento(id),

    CONSTRAINT fk_viagem_origem
        FOREIGN KEY (origem_poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT fk_viagem_destino
        FOREIGN KEY (destino_poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT fk_viagem_equipamento_carga
        FOREIGN KEY (equipamento_carga_id)
        REFERENCES equipamento(id),

    CONSTRAINT uq_viagem_codigo
        UNIQUE (codigo),

    CONSTRAINT ck_viagem_massa
        CHECK (massa IS NULL OR massa >= 0),

    CONSTRAINT ck_viagem_periodo
        CHECK (fim IS NULL OR fim > inicio),

    CONSTRAINT ck_viagem_status
        CHECK (
            status IN (
                'EM_ANDAMENTO',
                'FINALIZADA',
                'EM_CORRECAO',
                'EM_CONTINGENCIA',
                'AGUARDANDO_APROVACAO',
                'APROVADA'
            )
        )
);


-- ------------------------------------------------------------
-- EVENTO DA VIAGEM
-- ------------------------------------------------------------

CREATE TABLE evento_viagem (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    viagem_id BIGINT NOT NULL,

    tipo_evento VARCHAR(30) NOT NULL,

    data_hora TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    latitude NUMERIC(10,7),
    longitude NUMERIC(10,7),

    poligono_id BIGINT,

    operador_id BIGINT,
    equipamento_id BIGINT,

    observacao TEXT,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_evento_viagem
        FOREIGN KEY (viagem_id)
        REFERENCES viagem(id),

    CONSTRAINT fk_evento_poligono
        FOREIGN KEY (poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT fk_evento_operador
        FOREIGN KEY (operador_id)
        REFERENCES operador(id),

    CONSTRAINT fk_evento_equipamento
        FOREIGN KEY (equipamento_id)
        REFERENCES equipamento(id),

    CONSTRAINT ck_evento_tipo
        CHECK (
            tipo_evento IN (
                'MOVIMENTANDO_VAZIO',
                'CARREGANDO',
                'MOVIMENTANDO_CHEIO',
                'BASCULANDO'
            )
        )
);


-- ------------------------------------------------------------
-- ÍNDICES
-- ------------------------------------------------------------

CREATE INDEX idx_equipamento_mina
    ON equipamento(mina_id);

CREATE INDEX idx_operador_mina
    ON operador(mina_id);

CREATE INDEX idx_locacao_mina
    ON locacao(mina_id);

CREATE INDEX idx_alocacao_equipamento_equipamento
    ON alocacao_equipamento(equipamento_id);

CREATE INDEX idx_alocacao_equipamento_locacao
    ON alocacao_equipamento(locacao_id);

CREATE INDEX idx_alocacao_operador_operador
    ON alocacao_operador(operador_id);

CREATE INDEX idx_alocacao_operador_equipamento
    ON alocacao_operador(equipamento_id);

CREATE INDEX idx_viagem_locacao
    ON viagem(locacao_id);

CREATE INDEX idx_viagem_equipamento
    ON viagem(alocacao_equipamento_id);

CREATE INDEX idx_viagem_inicio
    ON viagem(inicio);

CREATE INDEX idx_evento_viagem
    ON evento_viagem(viagem_id);

CREATE INDEX idx_evento_data_hora
    ON evento_viagem(data_hora);

CREATE INDEX idx_evento_poligono
    ON evento_viagem(poligono_id);


COMMIT;
