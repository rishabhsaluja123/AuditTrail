using AuditTrail.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Audit.Models.DTO
{
    public class AuditRequest
    {
        [Required]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public AuditAction Action { get; set; }

        public object? ObjectBefore { get; set; }

        public object? ObjectAfter { get; set; }
    }
    public class AuditResponse
    {
        public int Id { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public AuditAction Action { get; set; }
        public List<FieldChangeDto> Changes { get; set; } = new List<FieldChangeDto>();
    }

    public class FieldChangeDto
    {
        public string FieldName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }

    public class AuditQueryRequest
    {
        public string? EntityName { get; set; }
        public string? UserId { get; set; }
        public AuditAction? Action { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
