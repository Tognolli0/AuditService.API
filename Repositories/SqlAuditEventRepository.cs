using AuditService.API.Contracts;
using AuditService.API.Data;
using AuditService.API.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AuditService.API.Repositories;

public sealed class SqlAuditEventRepository(string connectionString) : IAuditEventRepository
{
    public async Task EnsureSchemaAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(AuditSchema.CreateTableSql);
    }

    public async Task<long> CreateAsync(CreateAuditEventRequest request)
    {
        await EnsureSchemaAsync();

        const string sql = """
            INSERT INTO dbo.AuditEvents
            (
                ApplicationName,
                Usuario,
                Metodo,
                Endpoint,
                PayloadRequest,
                PayloadResponse,
                StatusCode,
                CorrelationId,
                Severity,
                Notes
            )
            OUTPUT INSERTED.Id
            VALUES
            (
                @ApplicationName,
                @Usuario,
                @Metodo,
                @Endpoint,
                @PayloadRequest,
                @PayloadResponse,
                @StatusCode,
                @CorrelationId,
                @Severity,
                @Notes
            );
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.ExecuteScalarAsync<long>(sql, request);
    }

    public async Task<IReadOnlyList<AuditEvent>> GetAsync(AuditEventQuery query)
    {
        await EnsureSchemaAsync();

        var filters = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(query.ApplicationName))
        {
            filters.Add("ApplicationName = @ApplicationName");
            parameters.Add("ApplicationName", query.ApplicationName);
        }

        if (!string.IsNullOrWhiteSpace(query.Usuario))
        {
            filters.Add("Usuario = @Usuario");
            parameters.Add("Usuario", query.Usuario);
        }

        if (query.StatusCode is not null)
        {
            filters.Add("StatusCode = @StatusCode");
            parameters.Add("StatusCode", query.StatusCode);
        }

        if (query.CreatedFromUtc is not null)
        {
            filters.Add("CreatedAtUtc >= @CreatedFromUtc");
            parameters.Add("CreatedFromUtc", query.CreatedFromUtc);
        }

        if (query.CreatedToUtc is not null)
        {
            filters.Add("CreatedAtUtc <= @CreatedToUtc");
            parameters.Add("CreatedToUtc", query.CreatedToUtc);
        }

        var safeLimit = Math.Clamp(query.Limit, 1, 200);
        parameters.Add("Limit", safeLimit);

        var whereClause = filters.Count > 0
            ? $"WHERE {string.Join(" AND ", filters)}"
            : string.Empty;

        var sql = $"""
            SELECT TOP (@Limit)
                Id,
                ApplicationName,
                Usuario,
                Metodo,
                Endpoint,
                PayloadRequest,
                PayloadResponse,
                StatusCode,
                CorrelationId,
                Severity,
                Notes,
                CreatedAtUtc
            FROM dbo.AuditEvents
            {whereClause}
            ORDER BY CreatedAtUtc DESC, Id DESC;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        var items = await connection.QueryAsync<AuditEvent>(sql, parameters);
        return items.ToList();
    }

    public async Task<AuditEvent?> GetByIdAsync(long id)
    {
        await EnsureSchemaAsync();

        const string sql = """
            SELECT
                Id,
                ApplicationName,
                Usuario,
                Metodo,
                Endpoint,
                PayloadRequest,
                PayloadResponse,
                StatusCode,
                CorrelationId,
                Severity,
                Notes,
                CreatedAtUtc
            FROM dbo.AuditEvents
            WHERE Id = @Id;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        return await connection.QuerySingleOrDefaultAsync<AuditEvent>(sql, new { Id = id });
    }

    public async Task<AuditEventSummary> GetSummaryAsync()
    {
        await EnsureSchemaAsync();

        const string summarySql = """
            SELECT
                COUNT(*) AS TotalEvents,
                SUM(CASE WHEN StatusCode >= 400 THEN 1 ELSE 0 END) AS ErrorEvents,
                COUNT(DISTINCT ApplicationName) AS DistinctApplications
            FROM dbo.AuditEvents;
            """;

        const string topApplicationsSql = """
            SELECT TOP (5)
                ApplicationName,
                COUNT(*) AS TotalEvents
            FROM dbo.AuditEvents
            GROUP BY ApplicationName
            ORDER BY COUNT(*) DESC, ApplicationName;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var summary = await connection.QuerySingleAsync<AuditEventSummary>(summarySql);
        var topApplications = await connection.QueryAsync<ApplicationAuditSummary>(topApplicationsSql);

        return new AuditEventSummary
        {
            TotalEvents = summary.TotalEvents,
            ErrorEvents = summary.ErrorEvents,
            DistinctApplications = summary.DistinctApplications,
            TopApplications = topApplications.ToList()
        };
    }
}
