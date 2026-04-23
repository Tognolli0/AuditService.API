using AuditService.API.Contracts;
using AuditService.API.Models;

namespace AuditService.API.Repositories;

public interface IAuditEventRepository
{
    Task EnsureSchemaAsync();
    Task<long> CreateAsync(CreateAuditEventRequest request);
    Task<IReadOnlyList<AuditEvent>> GetAsync(AuditEventQuery query);
    Task<AuditEvent?> GetByIdAsync(long id);
    Task<AuditEventSummary> GetSummaryAsync();
}
