# Modelo de Dados — FleetControl

## 1. Entidades principais

O núcleo operacional do sistema será composto inicialmente por:

- Operador
- Equipamento
- Locação
- Alocação de equipamento
- Alocação de operador
- Viagem
- Evento da viagem
- Polígono
- Checklist
- Ordem de serviço
- Correção
- Aprovação

---

## 2. Operador

Representa a pessoa que opera um equipamento.

Informações principais:

- id
- nome
- matrícula
- cartão
- status

Um operador pode operar equipamentos diferentes durante o turno.

Não pode possuir duas operações simultâneas.

---

## 3. Equipamento

Representa caminhões, escavadeiras e demais equipamentos operacionais.

Informações principais:

- id
- código
- nome
- modelo
- tipo
- status

Um equipamento pode:

- ficar sem operador;
- receber operadores diferentes;
- mudar de locação;
- participar de várias viagens ao longo do tempo.

---

## 4. Locação

Representa a configuração operacional.

Informações principais:

- id
- código
- origem/frente
- destino
- status
- data de início
- data de fim

Uma locação possui uma frente e um destino definidos.

Um equipamento só pode possuir uma locação ativa simultaneamente.

---

## 5. Alocação de equipamento

Representa o histórico da permanência do equipamento em uma locação.

Relacionamento:

EQUIPAMENTO 1:N ALOCACAO_EQUIPAMENTO N:1 LOCACAO

Informações principais:

- id
- equipamento_id
- locacao_id
- inicio
- fim
- status

---

## 6. Alocação de operador

Representa qual operador estava utilizando determinado equipamento.

Relacionamento:

OPERADOR 1:N ALOCACAO_OPERADOR N:1 EQUIPAMENTO

Informações principais:

- id
- operador_id
- equipamento_id
- início
- fim

Essa entidade preserva o histórico de troca de operador.

---

## 7. Viagem

Representa um ciclo completo de transporte.

Relacionamento principal:

ALOCACAO_EQUIPAMENTO 1:N VIAGEM

Informações principais:

- id
- equipamento_id
- locacao_id
- origem_id
- destino_id
- operador_id
- massa
- início
- fim
- status

Cada viagem possui identificação única.

---

## 8. Evento da viagem

Representa cada etapa operacional.

Relacionamento:

VIAGEM 1:N EVENTO_VIAGEM

Eventos possíveis inicialmente:

- MOVIMENTANDO_VAZIO
- CARREGANDO
- MOVIMENTANDO_CHEIO
- BASCULANDO

Informações:

- id
- viagem_id
- tipo_evento
- data_hora
- latitude
- longitude
- poligono_id

O evento BASCULANDO encerra a viagem.

---

## 9. Polígono

Representa uma área geográfica operacional.

Pode representar:

- frente de lavra;
- destino;
- outras áreas operacionais.

Informações:

- id
- nome
- tipo
- geometria
- status

Regra: polígonos operacionais não podem possuir sobreposição.

---

## 10. Estrutura inicial do relacionamento

OPERADOR
    │
    └──< ALOCACAO_OPERADOR >── EQUIPAMENTO
                                      │
                                      └──< ALOCACAO_EQUIPAMENTO >── LOCACAO
                                                    │
                                                    └──< VIAGEM
                                                           │
                                                           └──< EVENTO_VIAGEM

POLIGONO pode ser relacionado à:

- LOCACAO como origem ou destino;
- EVENTO_VIAGEM para identificação geográfica.

---

## 11. Próxima etapa

Após validar este modelo, o próximo passo será:

1. instalar/configurar PostgreSQL;
2. criar o banco `fleetcontrol`;
3. criar as tabelas;
4. definir PKs e FKs;
5. criar constraints para garantir as regras operacionais;
6. versionar tudo no Git.