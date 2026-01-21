AuditService.API - Central de Auditoria e Logs
📝 Descrição do Projeto

O AuditService.API é uma solução robusta desenvolvida em .NET 9 para centralizar a auditoria de acessos e logs de múltiplos sistemas corporativos, como o software SIGO. O objetivo principal é fornecer uma trilha de auditoria unificada, permitindo que diferentes aplicações (operacionais, comerciais ou de RH) registrem eventos de forma padronizada em um banco de dados SQL Server centralizado.

Este serviço é essencial para conformidade com a LGPD (Lei Geral de Proteção de Dados), permitindo rastrear quem acessou determinados dados, em qual horário e qual foi o resultado da operação.
🚀 Tecnologias Utilizadas

    .NET 9 (ASP.NET Core): Framework de alta performance para construção de APIs escaláveis.

    Dapper: Micro-ORM escolhido pela sua eficiência e velocidade superior na execução de comandos SQL.

    SQL Server 2019: Persistência de dados utilizando segurança integrada.

    Swagger (OpenAPI): Documentação interativa que facilita o teste e a integração por outros desenvolvedores.

    Programação Assíncrona (Async/Await): Implementação focada em alta disponibilidade e não bloqueio de threads.

🏗️ Arquitetura e Fluxo

A API atua como um middleware de auditoria. O fluxo de dados segue este padrão:

    Requisição: O sistema cliente envia um objeto AuditEntry via POST.

    Processamento: A API valida os dados e injeta a connectionString necessária para o acesso ao banco.

    Persistência: Utilizando o Dapper, o log é inserido de forma assíncrona na tabela AuditLogs.

🛠️ Como Configurar o Ambiente
Pré-requisitos

    .NET SDK 9.0 ou superior.

    SQL Server (Instância DEVELOPER02-NB\MSSQLSERVER2019).

Configuração do Banco de Dados

Execute o script abaixo no seu SSMS para preparar o ambiente:
SQL

CREATE DATABASE AuditDb;
GO
USE AuditDb;
GO
CREATE TABLE AuditLogs (
    Id INT PRIMARY KEY IDENTITY,
    ApplicationName VARCHAR(100),
    Usuario VARCHAR(100),
    Metodo VARCHAR(10),
    Endpoint VARCHAR(255),
    DataAcesso DATETIME DEFAULT GETDATE(),
    PayloadRequest NVARCHAR(MAX),
    StatusCode INT
);

Configuração da API

No arquivo appsettings.json, ajuste sua string de conexão:
JSON

"ConnectionStrings": {
  "DefaultConnection": "Server=DEVELOPER02-NB\\MSSQLSERVER2019;Database=AuditDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

🧪 Testando a API

    Execute o projeto: dotnet run.

    Acesse o Swagger: http://localhost:5169/swagger.

    Utilize o modelo de exemplo para enviar um log de teste:

JSON

{
  "applicationName": "Sistema_SIGO",
  "usuario": "Wisesystem",
  "metodo": "POST",
  "endpoint": "/api/pacientes",
  "payloadRequest": "{ 'id': 123, 'acao': 'consulta' }",
  "statusCode": 200
}
