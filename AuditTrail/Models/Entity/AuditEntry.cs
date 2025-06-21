using AuditTrail.Models.Enums;

namespace AuditTrail.Models.Entity
{
    public class AuditEntry
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public AuditAction Action { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> ChangedColumns { get; set; }
    }
}
