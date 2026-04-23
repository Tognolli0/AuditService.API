# AuditService.API

API em ASP.NET Core para registrar, consultar e resumir eventos de auditoria de multiplos sistemas em um ponto unico.

## O que o projeto entrega

- Recebimento padronizado de eventos de auditoria
- Persistencia em SQL Server com Dapper
- Consulta com filtros por aplicacao, usuario, status e periodo
- Resumo rapido para analise operacional
- Swagger como camada visual para exploracao e teste
- Script SQL versionado para reproducao do banco

## Stack

- ASP.NET Core / .NET 9
- SQL Server
- Dapper
- Swagger / OpenAPI

## Endpoints principais

- `POST /api/audit-events`
- `GET /api/audit-events`
- `GET /api/audit-events/{id}`
- `GET /api/audit-events/summary`
- `GET /healthz`

## Exemplo de payload

```json
{
  "applicationName": "PortalClinico",
  "usuario": "diogo.tognolli",
  "metodo": "POST",
  "endpoint": "/api/pacientes",
  "payloadRequest": "{ \"pacienteId\": 321 }",
  "payloadResponse": "{ \"success\": true }",
  "statusCode": 201,
  "correlationId": "req-2026-04-23-001",
  "severity": "Information",
  "notes": "Cadastro realizado com sucesso."
}
```

## Como rodar

1. Ajuste `ConnectionStrings:DefaultConnection`
2. Use `appsettings.Local.example.json` como base para ambiente local
3. Rode `dotnet run`
4. Acesse `/swagger`
5. Opcionalmente execute `database/create-audit-table.sql` manualmente

Observacao:
Ao iniciar, a API ja tenta garantir a existencia da tabela `dbo.AuditEvents`.

## Estrutura

- `Controllers/AuditEventsController.cs`: endpoints principais
- `Contracts`: payloads de entrada e filtros
- `Models`: entidades e resumo
- `Repositories`: persistencia com Dapper
- `database/create-audit-table.sql`: script versionado do schema

## Valor de portfolio

Esse projeto reforca um perfil back-end por mostrar:

- integracao com banco relacional
- rastreabilidade
- padronizacao de eventos
- filtros de consulta
- preocupacao com observabilidade e suporte operacional
