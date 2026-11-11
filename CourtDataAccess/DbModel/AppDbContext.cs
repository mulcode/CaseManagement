using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CourtDomain.Models;

namespace CourtDataAccess.DbModel
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
               base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Defendant> Defendants { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Petition> Petitions { get; set; }
        public DbSet<ArraignmentHearing> Arraignments { get; set; }
        public DbSet<Summon> Summons { get; set; }
        public DbSet<ReturnOfService> ReturnOfServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReturnOfService>(entity =>
            {
                // Older return-of-service rows may have no officer details recorded.
                // Keep these columns optional so NULL values can be read back.
                entity.Property(r => r.OfficerName)
                    .IsRequired(false);

                entity.Property(r => r.OfficerSignature)
                    .IsRequired(false);

                entity.Property(r => r.Reason)
                    .HasMaxLength(1000);

                entity.HasIndex(r => r.SummonId);

                // A return of service cannot exist without its summons. Deleting the
                // summons also removes its return records.
                entity.HasOne(r => r.Summon)
                    .WithMany()
                    .HasForeignKey(r => r.SummonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
