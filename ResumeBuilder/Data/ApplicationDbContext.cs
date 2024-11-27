using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Models;

namespace ResumeBuilder.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Skills> Skills { get; set; }
         public DbSet<WorkExperience> WorkExperience { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Certifications> Certifications { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }



    }
}
