# AuditService.API

API em ASP.NET Core para centralizar trilhas de auditoria e logs de multiplos sistemas corporativos em um ponto unico.

## Stack

- ASP.NET Core
- .NET 9
- SQL Server
- Swagger / OpenAPI

## Objetivo

Padronizar o recebimento de eventos de auditoria, facilitar rastreabilidade e apoiar cenarios de compliance e suporte operacional.

## Estrutura

- `Program.cs`: configuracao da API e endpoints
- `appsettings.json`: configuracao local
- `AuditService.API.csproj`: projeto principal

## Como rodar

1. Configure a string de conexao localmente.
2. Crie a tabela de auditoria no SQL Server.
3. Rode `dotnet run`.
4. Acesse o Swagger para validar os endpoints.
5. Use `appsettings.Local.example.json` como modelo de configuracao local.

## Exemplo de payload

```json
{
  "applicationName": "Sistema",
  "usuario": "WAYNE",
  "metodo": "POST",
  "endpoint": "/api/pacientes",
  "payloadRequest": "{ \"id\": 123, \"acao\": \"consulta\" }",
  "statusCode": 200
}
```

## Observacao

Este projeto tem bom valor de portfolio para perfil back-end por mostrar integracao, rastreabilidade e preocupacao com observabilidade.
Ele agora tambem expõe `GET /healthz` para verificacao simples de disponibilidade.
