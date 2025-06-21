using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using AuditTrail.Models.Entity;

namespace Audit.EntityDbContext
{
    public class AuditContext : DbContext
    {
        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
        }

        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<FieldChange> FieldChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EntityName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Action).IsRequired();

                entity.HasMany(e => e.Changes)
                      .WithOne(c => c.AuditEntry)
                      .HasForeignKey(c => c.AuditEntryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FieldChange>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FieldName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.OldValue).HasMaxLength(1000);
                entity.Property(e => e.NewValue).HasMaxLength(1000);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
