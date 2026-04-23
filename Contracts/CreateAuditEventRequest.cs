using System.ComponentModel.DataAnnotations;

namespace AuditService.API.Contracts;

public sealed class CreateAuditEventRequest
{
    [Required]
    [MaxLength(120)]
    public string ApplicationName { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Usuario { get; init; } = string.Empty;

    [Required]
    [MaxLength(16)]
    public string Metodo { get; init; } = string.Empty;

    [Required]
    [MaxLength(260)]
    public string Endpoint { get; init; } = string.Empty;

    [MaxLength(4000)]
    public string? PayloadRequest { get; init; }

    [MaxLength(4000)]
    public string? PayloadResponse { get; init; }

    [Range(100, 599)]
    public int StatusCode { get; init; }

    [MaxLength(80)]
    public string? CorrelationId { get; init; }

    [MaxLength(30)]
    public string? Severity { get; init; }

    [MaxLength(500)]
    public string? Notes { get; init; }
}
