# FleetControl

Sistema de controle e gestão operacional para mineração, desenvolvido para modelar alocações, viagens, apontamentos e operações de equipamentos.

## 🎯 Sobre o projeto

O FleetControl é um projeto desenvolvido com base em processos reais de operação de mineração.

O objetivo é centralizar e controlar informações relacionadas a:

- equipamentos;
- operadores;
- locações;
- alocações;
- viagens;
- eventos operacionais;
- checklists;
- manutenção;
- contingências;
- correções;
- geolocalização e polígonos.

O projeto está sendo desenvolvido com foco em regras de negócio, integridade dos dados e rastreabilidade das operações.

## 🏗️ Arquitetura

O projeto está sendo desenvolvido utilizando:

- **Backend:** C# / .NET
- **ORM:** Entity Framework Core
- **Banco de dados:** PostgreSQL
- **Geolocalização:** PostGIS / NetTopologySuite
- **Infraestrutura:** Docker
- **Frontend:** React *(em desenvolvimento)*

## 📊 Modelo operacional

O fluxo principal de uma viagem segue o ciclo:

```text
MOVIMENTANDO VAZIO
        ↓
CARREGANDO
        ↓
MOVIMENTANDO CHEIO
        ↓
BASCULANDO
        ↓
MOVIMENTANDO VAZIO