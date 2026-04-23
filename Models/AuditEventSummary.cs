namespace AuditService.API.Models;

public sealed class AuditEventSummary
{
    public int TotalEvents { get; init; }
    public int ErrorEvents { get; init; }
    public int DistinctApplications { get; init; }
    public IReadOnlyList<ApplicationAuditSummary> TopApplications { get; init; } = [];
}

public sealed class ApplicationAuditSummary
{
    public string ApplicationName { get; init; } = string.Empty;
    public int TotalEvents { get; init; }
}
