# Regras de Negócio — FleetControl

## 1. Objetivo

Sistema de controle e gestão operacional para mineração, voltado para alocações, equipamentos, operadores, viagens, apontamentos e operações.

---

## 2. Operadores e equipamentos

- Um equipamento pode ficar sem operador.
- Um equipamento não pode ter mais de um operador simultaneamente.
- Um operador pode trocar de equipamento durante o turno.
- Um operador não pode operar dois equipamentos simultaneamente.
- Um equipamento pode ficar parado sem operador e posteriormente receber outro operador.

A troca de operador pode ocorrer durante o turno, respeitando o histórico da operação.

---

## 3. Locação

A locação define a operação dos equipamentos.

Uma locação possui:

- Frente de lavra/origem.
- Destino.
- Equipamentos alocados.

Um equipamento só pode possuir uma locação ativa por vez.

Equipamentos podem entrar ou sair de uma locação durante sua operação.

A frente e o destino são os elementos fixos principais da locação.

---

## 4. Troca de equipamento

Um equipamento pode ser substituído dentro da locação.

Exemplo:

- EH682 está operando.
- EH682 informa ao controle que será substituído.
- O controle altera a alocação.
- EH743 passa a assumir a operação.

A troca não pode ocorrer no meio do processo de carregamento.

A troca deve ocorrer após a finalização do carregamento, quando o caminhão entra no estado MOVIMENTANDO_CHEIO.

---

## 5. Ciclo operacional da viagem

O ciclo operacional segue a sequência:

MOVIMENTANDO_VAZIO
↓
CARREGANDO
↓
MOVIMENTANDO_CHEIO
↓
BASCULANDO
↓
MOVIMENTANDO_VAZIO

Cada viagem é única.

O evento BASCULANDO finaliza a viagem atual.

Após a finalização, o equipamento pode iniciar uma nova viagem em MOVIMENTANDO_VAZIO.

---

## 6. Eventos da viagem

Os principais eventos são:

- MOVIMENTANDO_VAZIO
- CARREGANDO
- MOVIMENTANDO_CHEIO
- BASCULANDO

Cada evento deve possuir registro de data e hora.

Os eventos formam o histórico operacional da viagem.

---

## 7. Massa

Cada equipamento possui balança embarcada.

A massa registrada na viagem é obtida a partir do equipamento.

---

## 8. Polígonos e localização

A operação utiliza polígonos geográficos para identificar áreas operacionais.

Regras:

- Um apontamento dentro de um polígono pode identificar automaticamente sua localização.
- Não podem existir dois polígonos sobrepostos.
- Um apontamento próximo da borda pode exigir identificação ou confirmação.
- Um apontamento fora de qualquer polígono gera erro.

---

## 9. Correção e contingência

Uma viagem iniciada deve ser finalizada.

O cancelamento operacional não é permitido.

Registros podem sofrer correção.

A correção pode ser realizada pelo Controle ou pelo cliente EuroChem.

Viagens que passam por:

- correção;
- contingência;

podem exigir aprovação.

---

## 10. Checklist

Equipamentos podem possuir bloqueio operacional relacionado ao checklist.

Quando bloqueado, o equipamento permanece impedido de operar até o tratamento da ocorrência por Ordem de Serviço.

---

## 11. Princípios do histórico

O sistema deve preservar o histórico de:

- operador;
- equipamento;
- locação;
- viagem;
- evento;
- alteração;
- correção;
- contingência.

Alterações operacionais não devem apagar o histórico original.