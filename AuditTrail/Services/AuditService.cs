using AuditTrail.EntityDbContext;
using AuditTrail.Interfaces.IServices;
using AuditTrail.Models.Entity;
using AuditTrail.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AuditTrail.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAuditAsync<T>(T oldObject, T newObject, AuditAction action, string userId, string entityName)
        {
            var changedColumns = new Dictionary<string, object>();

            if (action == AuditAction.Updated)
            {
                changedColumns = CompareObjects(oldObject, newObject);
                if (!changedColumns.Any()) return; // No changes detected
            }
            else if (action == AuditAction.Created || action == AuditAction.Deleted)
            {
                changedColumns = GetAllProperties(newObject);
            }

            var auditEntry = new AuditEntry
            {
                EntityName = entityName,
                Action = action,
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                ChangedColumns = changedColumns
            };

            _context.AuditEntries.Add(auditEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditEntry>> GetAuditTrailAsync(string entityName, int page, int pageSize)
        {
            return await _context.AuditEntries
                .Where(a => a.EntityName == entityName)
                .OrderByDescending(a => a.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private Dictionary<string, object> CompareObjects<T>(T oldObject, T newObject)
        {
            var changes = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var oldValue = property.GetValue(oldObject);
                var newValue = property.GetValue(newObject);

                if (!Equals(oldValue, newValue))
                {
                    changes.Add(property.Name, newValue);
                }
            }

            return changes;
        }

        private Dictionary<string, object> GetAllProperties<T>(T obj)
        {
            var properties = new Dictionary<string, object>();
            foreach (var property in typeof(T).GetProperties())
            {
                properties.Add(property.Name, property.GetValue(obj));
            }
            return properties;
        }
    }
}
