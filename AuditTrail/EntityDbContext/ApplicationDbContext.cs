using AuditTrail.Models.Entity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuditTrail.EntityDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuditEntry> AuditEntries { get; set; }
    }
}
