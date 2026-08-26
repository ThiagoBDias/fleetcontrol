# Regras de Integridade — FleetControl

## 1. Exclusividade de operação

### Equipamento

Um equipamento não pode possuir duas alocações ativas simultaneamente.

Regra:

> Para o mesmo equipamento, só pode existir uma alocação sem data de fim.

---

### Operador

Um operador não pode operar dois equipamentos simultaneamente.

Regra:

> Para o mesmo operador, não pode existir sobreposição de períodos de alocação.

Um equipamento pode permanecer sem operador.

A troca de operador pode ocorrer a qualquer momento do turno.

---

## 2. Exclusividade de locação

Um equipamento só pode estar em uma locação por vez.

Uma nova locação só pode assumir o equipamento após o encerramento da alocação anterior.

Exemplo:

EH682
↓
Locação A encerrada
↓
Locação B iniciada

Não é permitido:

EH682
├── Locação A ativa
└── Locação B ativa

---

## 3. Troca de equipamento na locação

Um equipamento pode ser substituído por outro.

Exemplo:

EH682 informa indisponibilidade.

Controle operacional:

1. encerra a alocação do EH682;
2. registra o motivo da substituição;
3. encerra a relação operacional anterior;
4. aloca o EH743;
5. o EH743 passa a operar na mesma locação.

A substituição não pode ocorrer durante o evento:

CARREGANDO.

A troca de equipamento só pode ocorrer após o encerramento do carregamento.

---

## 4. Ciclo da viagem

O ciclo operacional permitido é:

MOVIMENTANDO_VAZIO
        ↓
CARREGANDO
        ↓
MOVIMENTANDO_CHEIO
        ↓
BASCULANDO
        ↓
MOVIMENTANDO_VAZIO

Regras:

- uma viagem possui identificador único;
- uma viagem é iniciada a partir de uma locação válida;
- a viagem possui origem e destino;
- o evento CARREGANDO ocorre na origem;
- o evento BASCULANDO ocorre no destino;
- BASCULANDO finaliza a viagem;
- uma viagem iniciada não pode permanecer indefinidamente aberta sem tratamento operacional.

---

## 5. Geolocalização

Cada evento operacional pode possuir:

- latitude;
- longitude;
- data e hora;
- polígono identificado.

Regras:

### Dentro de um polígono

O evento pode ser associado automaticamente ao polígono.

### Próximo da borda

O sistema deve identificar a proximidade e permitir confirmação operacional quando necessário.

### Sobreposição

Não podem existir dois polígonos operacionais ativos ocupando a mesma área.

### Fora de qualquer polígono

O apontamento deve gerar erro ou contingência.

---

## 6. Origem e destino

A locação define:

- uma origem/frente operacional;
- um destino operacional.

Esses elementos são a referência operacional da viagem.

Uma viagem deve nascer vinculada à locação vigente do equipamento.

---

## 7. Massa

Cada equipamento possui sistema interno de pesagem.

A massa registrada pertence à viagem.

Após o carregamento, a massa deve ser associada ao ciclo antes da conclusão da viagem.

---

## 8. Correção e contingência

Apontamentos enviados para:

- correção;
- contingência;

podem passar por processo de aprovação.

Alterações devem preservar rastreabilidade.

O sistema deve registrar:

- valor original;
- valor corrigido;
- responsável;
- data e hora;
- motivo.

---

## 9. Cancelamento

Uma viagem operacional iniciada deve seguir o fluxo até sua finalização.

O controle operacional pode excluir ou tratar registros conforme regra de negócio e permissão.

A exclusão física de dados históricos deve ser evitada.

Preferência:

> Cancelamento lógico com rastreabilidade.

---

## 10. Checklist

Um equipamento pode ficar bloqueado para operação.

Enquanto o checklist ou a situação operacional exigir bloqueio:

> o equipamento não pode iniciar uma nova viagem.

A liberação depende da regularização definida pela Ordem de Serviço ou processo operacional correspondente.

---

## 11. Auditoria

Alterações críticas devem ser rastreáveis.

Registrar:

- quem realizou;
- quando realizou;
- operação executada;
- valor anterior;
- valor posterior;
- motivo, quando aplicável.

---

## 12. Princípio de integridade

Nenhuma regra crítica deve depender exclusivamente da interface.

As regras que protegem a consistência operacional devem ser implementadas, sempre que possível, também no banco de dados.