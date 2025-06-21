using Audit.Models.DTO;
using Audit.EntityDbContext;
using Audit.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using AuditTrail.EntityDbContext;
using AuditTrail.Models.Entity;
using AuditTrail.Models.Enums;

namespace Audit.Services
{
    public class AuditService : IAuditService

    {
        private readonly AuditContext _context;

        public AuditService(AuditContext context)
        {
            _context = context;
        }

        public async Task<AuditResponse> CreateAuditAsync(AuditRequest request)
        {
            try
            {
                var auditEntry = new AuditEntry
                {
                    EntityName = request.EntityName,
                    UserId = request.UserId,
                    Timestamp = DateTime.UtcNow,
                    Action = request.Action,
                    Changes = new List<FieldChange>()
                };

                switch (request.Action)
                {
                    case AuditAction.Created:
                        auditEntry.Changes = GetCreatedChanges(request.ObjectAfter);
                        break;

                    case AuditAction.Updated:
                        auditEntry.Changes = GetUpdatedChanges(request.ObjectBefore, request.ObjectAfter);
                        break;

                    case AuditAction.Deleted:
                        auditEntry.Changes = GetDeletedChanges(request.ObjectBefore);
                        break;
                }

                _context.AuditEntries.Add(auditEntry);
                await _context.SaveChangesAsync();

                return MapToResponse(auditEntry);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<PagedResult<AuditResponse>> GetAuditsAsync(AuditQueryRequest query)
        {
            try
            {
                var queryable = _context.AuditEntries
                .Include(a => a.Changes)
                .AsQueryable();

                if (!string.IsNullOrEmpty(query.EntityName))
                    queryable = queryable.Where(a => a.EntityName == query.EntityName);

                if (!string.IsNullOrEmpty(query.UserId))
                    queryable = queryable.Where(a => a.UserId == query.UserId);

                if (query.Action.HasValue)
                    queryable = queryable.Where(a => a.Action == query.Action.Value);

                if (query.FromDate.HasValue)
                    queryable = queryable.Where(a => a.Timestamp >= query.FromDate.Value);

                if (query.ToDate.HasValue)
                    queryable = queryable.Where(a => a.Timestamp <= query.ToDate.Value);

                var totalCount = await queryable.CountAsync();

                var items = await queryable
                    .OrderByDescending(a => a.Timestamp)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync();

                return new PagedResult<AuditResponse>
                {
                    Items = items.Select(MapToResponse).ToList(),
                    TotalCount = totalCount,
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize
                };
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<AuditResponse?> GetAuditByIdAsync(int id)
        {
            try
            {
                var entry = await _context.AuditEntries
                .Include(a => a.Changes)
                .FirstOrDefaultAsync(a => a.Id == id);

                return entry != null ? MapToResponse(entry) : null;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private List<FieldChange> GetCreatedChanges(object? objectAfter)
        {
            try
            {
                var changes = new List<FieldChange>();

                if (objectAfter == null)
                    return changes;

                var properties = GetObjectProperties(objectAfter);

                foreach (var (name, value) in properties)
                {
                    changes.Add(new FieldChange
                    {
                        FieldName = name,
                        OldValue = null,
                        NewValue = value
                    });
                }

                return changes;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private List<FieldChange> GetUpdatedChanges(object? objectBefore, object? objectAfter)
        {
            try
            {
                var changes = new List<FieldChange>();

                if (objectBefore == null || objectAfter == null)
                    return changes;

                var beforeProps = GetObjectProperties(objectBefore);
                var afterProps = GetObjectProperties(objectAfter);

                foreach (var beforeProp in beforeProps)
                {
                    if (afterProps.TryGetValue(beforeProp.Key, out var newValue))
                    {
                        if (beforeProp.Value != newValue)
                        {
                            changes.Add(new FieldChange
                            {
                                FieldName = beforeProp.Key,
                                OldValue = beforeProp.Value,
                                NewValue = newValue
                            });
                        }
                    }
                }

                return changes;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private List<FieldChange> GetDeletedChanges(object? objectBefore)
        {
            try
            {
                var changes = new List<FieldChange>();

                if (objectBefore == null)
                    return changes;

                var properties = GetObjectProperties(objectBefore);

                foreach (var (name, value) in properties)
                {
                    changes.Add(new FieldChange
                    {
                        FieldName = name,
                        OldValue = value,
                        NewValue = null
                    });
                }

                return changes;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private Dictionary<string, string?> GetObjectProperties(object obj)
        {
            try
            {
                var properties = new Dictionary<string, string?>();

                if (obj is JsonElement jsonElement)
                {
                    return GetJsonElementProperties(jsonElement);
                }

                var type = obj.GetType();
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    var value = prop.GetValue(obj);
                    properties[prop.Name] = value?.ToString();
                }

                return properties;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private Dictionary<string, string?> GetJsonElementProperties(JsonElement element)
        {
            try
            {
                var properties = new Dictionary<string, string?>();

                if (element.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in element.EnumerateObject())
                    {
                        properties[property.Name] = property.Value.ToString();
                    }
                }

                return properties;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private static AuditResponse MapToResponse(AuditEntry entry)
        {
            try
            {
                return new AuditResponse
                {
                    Id = entry.Id,
                    EntityName = entry.EntityName,
                    UserId = entry.UserId,
                    Timestamp = entry.Timestamp,
                    Action = entry.Action,
                    Changes = entry.Changes.Select(c => new FieldChangeDto
                    {
                        FieldName = c.FieldName,
                        OldValue = c.OldValue,
                        NewValue = c.NewValue
                    }).ToList()
                };
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
