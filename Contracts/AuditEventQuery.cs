namespace AuditService.API.Contracts;

public sealed class AuditEventQuery
{
    public string? ApplicationName { get; init; }
    public string? Usuario { get; init; }
    public int? StatusCode { get; init; }
    public DateTime? CreatedFromUtc { get; init; }
    public DateTime? CreatedToUtc { get; init; }
    public int Limit { get; init; } = 50;
}
