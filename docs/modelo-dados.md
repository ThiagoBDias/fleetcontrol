# Modelo de Dados — FleetControl

## 1. Visão geral

O FleetControl é um sistema de controle e gestão operacional para mineração.

O núcleo do sistema controla:

- equipamentos;
- operadores;
- locações;
- alocações;
- viagens;
- eventos operacionais;
- polígonos geográficos;
- checklist e bloqueios;
- ordem de serviço;
- correções;
- contingências;
- aprovações.

O modelo foi estruturado para preservar o histórico operacional e impedir inconsistências como:

- equipamento em duas locações simultaneamente;
- operador operando dois equipamentos simultaneamente;
- caminhão pertencendo a duas frentes simultaneamente;
- troca de equipamento no meio de uma viagem;
- apontamento fora de área operacional válida.

---

# 2. Entidades principais

- Empresa
- Mina
- Operador
- Modelo de equipamento
- Equipamento
- Polígono
- Locação
- Alocação de equipamento
- Alocação de operador
- Viagem
- Evento da viagem
- Checklist
- Ordem de serviço
- Correção
- Contingência
- Aprovação

---

# 3. Empresa

Representa a empresa responsável pela operação.

Informações principais:

- id
- nome
- status
- data de criação

Uma empresa pode possuir uma ou mais minas.

Relacionamento:

EMPRESA 1:N MINA

---

# 4. Mina

Representa uma unidade operacional.

Informações principais:

- id
- empresa_id
- nome
- código
- status

Relacionamentos:

MINA 1:N OPERADOR

MINA 1:N EQUIPAMENTO

MINA 1:N POLÍGONO

MINA 1:N LOCAÇÃO

---

# 5. Operador

Representa o colaborador responsável pela operação de um equipamento.

Informações principais:

- id
- mina_id
- nome
- matrícula
- cartão
- status

Regras:

- um operador pode operar equipamentos diferentes ao longo do turno;
- um operador não pode operar dois equipamentos simultaneamente;
- um equipamento pode permanecer sem operador;
- a troca de operador pode ocorrer durante o turno;
- o histórico das trocas deve ser preservado.

Relacionamento:

OPERADOR 1:N ALOCACAO_OPERADOR

---

# 6. Modelo de equipamento

Representa o modelo/tipo do equipamento.

Informações principais:

- id
- nome
- tipo
- status

Exemplos:

- caminhão;
- escavadeira;
- carregadeira;
- equipamento de apoio.

Relacionamento:

MODELO_EQUIPAMENTO 1:N EQUIPAMENTO

---

# 7. Equipamento

Representa um equipamento físico da operação.

Informações principais:

- id
- mina_id
- modelo_id
- código
- nome
- status
- possui_balanca
- bloqueado

Um equipamento pode:

- ficar sem operador;
- receber operadores diferentes;
- mudar de locação;
- participar de várias viagens;
- ser substituído por outro equipamento em uma locação.

Regra fundamental:

> Um equipamento não pode estar alocado simultaneamente em duas locações.

Relacionamentos:

EQUIPAMENTO 1:N ALOCACAO_EQUIPAMENTO

EQUIPAMENTO 1:N ALOCACAO_OPERADOR

---

# 8. Polígono

Representa uma área geográfica operacional.

Tipos principais:

- FRENTE
- DESTINO
- AREA_OPERACIONAL

Exemplos:

- Frente P12;
- Frente P9;
- Depósito de Estéril Cava A.

Informações principais:

- id
- mina_id
- nome
- tipo
- geometria
- status

A geometria será implementada utilizando PostGIS.

Regras:

- um apontamento deve ser associado ao polígono correspondente;
- o sistema deve identificar quando o equipamento está dentro de um polígono;
- o sistema deve identificar situações próximas à borda;
- não podem existir polígonos operacionais incompatíveis ocupando a mesma área;
- apontamento fora de qualquer polígono deve gerar erro ou entrar em contingência.

Relacionamentos:

POLÍGONO 1:N LOCAÇÃO

POLÍGONO 1:N EVENTO_VIAGEM

---

# 9. Locação

Representa uma configuração operacional ativa.

Uma locação define:

- uma frente de lavra/origem;
- um destino.

Informações principais:

- id
- mina_id
- origem_poligono_id
- destino_poligono_id
- início
- fim
- status

Exemplo:

```text
LOCAÇÃO
Frente P12
     ↓
Depósito de Estéril Cava A
