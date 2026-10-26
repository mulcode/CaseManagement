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

    }
}
