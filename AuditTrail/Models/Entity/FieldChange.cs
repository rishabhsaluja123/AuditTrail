using System.ComponentModel.DataAnnotations;

namespace AuditTrail.Models.Entity
{
    public class FieldChange
    {
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public int AuditEntryId { get; set; }

        public AuditEntry AuditEntry { get; set; } = null!;
    }
}
