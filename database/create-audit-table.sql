IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'AuditEvents' AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE dbo.AuditEvents (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        ApplicationName NVARCHAR(120) NOT NULL,
        Usuario NVARCHAR(120) NOT NULL,
        Metodo NVARCHAR(16) NOT NULL,
        Endpoint NVARCHAR(260) NOT NULL,
        PayloadRequest NVARCHAR(4000) NULL,
        PayloadResponse NVARCHAR(4000) NULL,
        StatusCode INT NOT NULL,
        CorrelationId NVARCHAR(80) NULL,
        Severity NVARCHAR(30) NULL,
        Notes NVARCHAR(500) NULL,
        CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_AuditEvents_CreatedAtUtc DEFAULT SYSUTCDATETIME()
    );
END;
