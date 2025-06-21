using AuditTrail.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuditTrail.Models.Entity
{
    public class AuditEntry
    {
        public int Id { get; set; }

        [Required]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public AuditAction Action { get; set; }

        public List<FieldChange> Changes { get; set; } = new List<FieldChange>();
    }
}
