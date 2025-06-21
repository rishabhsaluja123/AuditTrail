using AuditTrail.Models.Enums;

namespace AuditTrail.Models.DTO
{
    public class AuditRequest<T>
    {
        public T OldObject { get; set; }
        public T NewObject { get; set; }
        public AuditAction Action { get; set; }
        public string UserId { get; set; }
        public string EntityName { get; set; }
    }
}
