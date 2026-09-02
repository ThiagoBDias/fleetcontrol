-- ============================================================
-- FleetControl
-- Schema inicial do núcleo operacional
-- PostgreSQL + PostGIS
-- ============================================================

BEGIN;


-- ============================================================
-- EXTENSÕES
-- ============================================================

CREATE EXTENSION IF NOT EXISTS btree_gist;
CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS postgis;


-- ============================================================
-- EMPRESA
-- ============================================================

CREATE TABLE empresa (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    nome VARCHAR(150) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW()
);


-- ============================================================
-- MINA
-- ============================================================

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
        UNIQUE (empresa_id, codigo),

    CONSTRAINT uq_mina_id_empresa
        UNIQUE (id, empresa_id)
);


-- ============================================================
-- OPERADOR
-- ============================================================

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
        CHECK (
            status IN (
                'ATIVO',
                'INATIVO',
                'BLOQUEADO'
            )
        )
);


-- ============================================================
-- MODELO DE EQUIPAMENTO
-- ============================================================

CREATE TABLE modelo_equipamento (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    nome VARCHAR(150) NOT NULL,
    tipo VARCHAR(50) NOT NULL,

    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_modelo_equipamento_nome
        UNIQUE (nome)
);


-- ============================================================
-- EQUIPAMENTO
-- ============================================================

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

    CONSTRAINT uq_equipamento_id_mina
        UNIQUE (id, mina_id),

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


-- ============================================================
-- POLÍGONO
-- ============================================================

CREATE TABLE poligono (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    mina_id BIGINT NOT NULL,

    nome VARCHAR(150) NOT NULL,
    tipo VARCHAR(30) NOT NULL,

    geometria GEOMETRY(POLYGON, 4326) NOT NULL,

    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_poligono_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT uq_poligono_id_mina
        UNIQUE (id, mina_id),

    CONSTRAINT ck_poligono_tipo
        CHECK (
            tipo IN (
                'FRENTE',
                'DESTINO',
                'AREA_OPERACIONAL'
            )
        ),

    CONSTRAINT ck_poligono_geometria_valida
        CHECK (
            ST_IsValid(geometria)
        )
);


-- ============================================================
-- MATERIAL
-- ============================================================

CREATE TABLE material (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    mina_id BIGINT NOT NULL,

    nome VARCHAR(150) NOT NULL,

    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_material_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT uq_material_nome
        UNIQUE (mina_id, nome),

    CONSTRAINT uq_material_id_mina
        UNIQUE (id, mina_id)
);


-- ============================================================
-- LITOLOGIA
-- ============================================================

CREATE TABLE litologia (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    mina_id BIGINT NOT NULL,
    material_id BIGINT NOT NULL,

    nome VARCHAR(150) NOT NULL,
    tipo VARCHAR(100),

    ativo BOOLEAN NOT NULL DEFAULT TRUE,

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_litologia_mina
        FOREIGN KEY (mina_id)
        REFERENCES mina(id),

    CONSTRAINT fk_litologia_material
        FOREIGN KEY (material_id)
        REFERENCES material(id),

    CONSTRAINT uq_litologia_nome
        UNIQUE (mina_id, nome),

    CONSTRAINT uq_litologia_id_mina
        UNIQUE (id, mina_id)
);


-- ============================================================
-- LOCAÇÃO
-- ============================================================
--
-- Define:
--   - origem / frente
--   - destino
--   - período da operação
--
-- Exemplo:
--   Frente 10 -> Britador
--
-- A locação possui histórico temporal.
-- ============================================================

CREATE TABLE locacao (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    mina_id BIGINT NOT NULL,

    codigo VARCHAR(50) NOT NULL,

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
        FOREIGN KEY (origem_poligono_id, mina_id)
        REFERENCES poligono(id, mina_id),

    CONSTRAINT fk_locacao_destino
        FOREIGN KEY (destino_poligono_id, mina_id)
        REFERENCES poligono(id, mina_id),

    CONSTRAINT uq_locacao_codigo
        UNIQUE (mina_id, codigo),

    CONSTRAINT uq_locacao_integridade
        UNIQUE (
            id,
            origem_poligono_id,
            destino_poligono_id
        ),

    CONSTRAINT ck_locacao_periodo
        CHECK (
            fim IS NULL OR fim > inicio
        ),

    CONSTRAINT ck_locacao_status
        CHECK (
            status IN (
                'ATIVA',
                'ENCERRADA',
                'CANCELADA'
            )
        ),

    CONSTRAINT ck_locacao_origem_destino
        CHECK (
            origem_poligono_id <> destino_poligono_id
        )
);


-- ============================================================
-- ALOCAÇÃO DE EQUIPAMENTO
-- ============================================================
--
-- TRANSPORTE = caminhão
-- CARGA      = escavadeira/carregadeira
-- APOIO      = equipamento auxiliar
--
-- Mantém histórico de alocação.
-- ============================================================

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

    CONSTRAINT uq_alocacao_equipamento_integridade
        UNIQUE (
            id,
            locacao_id
        ),

    CONSTRAINT ck_alocacao_equipamento_funcao
        CHECK (
            funcao IN (
                'TRANSPORTE',
                'CARGA',
                'APOIO'
            )
        ),

    CONSTRAINT ck_alocacao_equipamento_periodo
        CHECK (
            fim IS NULL OR fim > inicio
        )
);


-- ============================================================
-- REGRA:
-- Um equipamento não pode estar em duas alocações
-- simultaneamente.
-- ============================================================

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


-- ============================================================
-- ALOCAÇÃO DE OPERADOR
-- ============================================================
--
-- Registra qual operador estava operando qual equipamento.
--
-- Um equipamento pode ficar sem operador.
-- Um operador pode trocar de equipamento.
-- ============================================================

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
        CHECK (
            fim IS NULL OR fim > inicio
        )
);


-- ============================================================
-- REGRA:
-- Um operador não pode operar dois equipamentos
-- simultaneamente.
-- ============================================================

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


-- ============================================================
-- VIAGEM
-- ============================================================
--
-- Ciclo:
--
-- MOVIMENTANDO VAZIO
--        ↓
-- CARREGANDO
--        ↓
-- MOVIMENTANDO CHEIO
--        ↓
-- BASCULANDO
--        ↓
-- FINALIZA
--
-- Cada ciclo representa uma nova viagem.
--
-- O operador não fica fixado diretamente na viagem.
-- Ele é registrado nos eventos.
-- ============================================================

CREATE TABLE viagem (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    codigo UUID NOT NULL DEFAULT gen_random_uuid(),

    locacao_id BIGINT NOT NULL,
    alocacao_equipamento_id BIGINT NOT NULL,

    origem_poligono_id BIGINT NOT NULL,
    destino_poligono_id BIGINT NOT NULL,

    litologia_id BIGINT,

    -- Equipamento que realizou o carregamento.
    equipamento_carga_id BIGINT,

    massa NUMERIC(12,2),

    inicio TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    fim TIMESTAMPTZ,

    status VARCHAR(30) NOT NULL DEFAULT 'EM_ANDAMENTO',

    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_viagem_locacao
        FOREIGN KEY (
            locacao_id,
            origem_poligono_id,
            destino_poligono_id
        )
        REFERENCES locacao (
            id,
            origem_poligono_id,
            destino_poligono_id
        ),

    CONSTRAINT fk_viagem_alocacao_equipamento
        FOREIGN KEY (
            alocacao_equipamento_id,
            locacao_id
        )
        REFERENCES alocacao_equipamento (
            id,
            locacao_id
        ),

    CONSTRAINT fk_viagem_origem
        FOREIGN KEY (origem_poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT fk_viagem_destino
        FOREIGN KEY (destino_poligono_id)
        REFERENCES poligono(id),

    CONSTRAINT fk_viagem_litologia
        FOREIGN KEY (litologia_id)
        REFERENCES litologia(id),

    CONSTRAINT fk_viagem_equipamento_carga
        FOREIGN KEY (equipamento_carga_id)
        REFERENCES equipamento(id),

    CONSTRAINT uq_viagem_codigo
        UNIQUE (codigo),

    CONSTRAINT ck_viagem_massa
        CHECK (
            massa IS NULL OR massa >= 0
        ),

    CONSTRAINT ck_viagem_periodo
        CHECK (
            fim IS NULL OR fim > inicio
        ),

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


-- ============================================================
-- EVENTO DA VIAGEM
-- ============================================================

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
        ),

    CONSTRAINT ck_evento_latitude
        CHECK (
            latitude IS NULL
            OR latitude BETWEEN -90 AND 90
        ),

    CONSTRAINT ck_evento_longitude
        CHECK (
            longitude IS NULL
            OR longitude BETWEEN -180 AND 180
        )
);


-- ============================================================
-- ÍNDICES
-- ============================================================

CREATE INDEX idx_equipamento_mina
    ON equipamento(mina_id);

CREATE INDEX idx_operador_mina
    ON operador(mina_id);

CREATE INDEX idx_locacao_mina
    ON locacao(mina_id);

CREATE INDEX idx_locacao_status
    ON locacao(status);

CREATE INDEX idx_alocacao_equipamento_equipamento
    ON alocacao_equipamento(equipamento_id);

CREATE INDEX idx_alocacao_equipamento_locacao
    ON alocacao_equipamento(locacao_id);

CREATE INDEX idx_alocacao_equipamento_periodo
    ON alocacao_equipamento(inicio, fim);

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

CREATE INDEX idx_viagem_status
    ON viagem(status);

CREATE INDEX idx_evento_viagem
    ON evento_viagem(viagem_id);

CREATE INDEX idx_evento_data_hora
    ON evento_viagem(data_hora);

CREATE INDEX idx_evento_poligono
    ON evento_viagem(poligono_id);

CREATE INDEX idx_evento_equipamento
    ON evento_viagem(equipamento_id);

CREATE INDEX idx_evento_operador
    ON evento_viagem(operador_id);

CREATE INDEX idx_poligono_geometria
    ON poligono
    USING GIST (geometria);

CREATE INDEX idx_poligono_mina_tipo
    ON poligono(mina_id, tipo);


-- ============================================================
-- FINALIZAÇÃO
-- ============================================================
COMMIT;