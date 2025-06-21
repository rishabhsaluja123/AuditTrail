using Audit.Models.DTO;

namespace Audit.Interfaces.IServices
{
    public interface IAuditService
    {
        Task<AuditResponse> CreateAuditAsync(AuditRequest request);
        Task<PagedResult<AuditResponse>> GetAuditsAsync(AuditQueryRequest query);
        Task<AuditResponse?> GetAuditByIdAsync(int id);
    }
}
