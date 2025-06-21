
using Audit.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuditTrail.EntityDbContext
{
    public class AuditContextFactory : IDesignTimeDbContextFactory<AuditContext>
    {
        public AuditContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuditContext>();

            // Use a default connection string for design-time
            optionsBuilder.UseSqlServer("Server=DESKTOP-E48N35M\\SQLEXPRESS;Database=AuditTrailDb;Trusted_Connection=true;MultipleActiveResultSets=true");

            return new AuditContext(optionsBuilder.Options);
        }
    }
}


