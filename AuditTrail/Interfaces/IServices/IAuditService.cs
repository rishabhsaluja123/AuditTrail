using AuditTrail.Models.Entity;
using AuditTrail.Models.Enums;

namespace AuditTrail.Interfaces.IServices
{
    public interface IAuditService
    {
        Task LogAuditAsync<T>(T oldObject, T newObject, AuditAction action, string userId, string entityName);
        Task<IEnumerable<AuditEntry>> GetAuditTrailAsync(string entityName, int page, int pageSize);
    }
}
