namespace AuditService.API.Models;

public sealed class AuditEvent
{
    public long Id { get; init; }
    public string ApplicationName { get; init; } = string.Empty;
    public string Usuario { get; init; } = string.Empty;
    public string Metodo { get; init; } = string.Empty;
    public string Endpoint { get; init; } = string.Empty;
    public string? PayloadRequest { get; init; }
    public string? PayloadResponse { get; init; }
    public int StatusCode { get; init; }
    public string? CorrelationId { get; init; }
    public string? Severity { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
