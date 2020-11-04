using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BDSA2020.Entities
{
    public class Context : DbContext, IContext
    {
        public DbSet<Student> Students { get; set; }

        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Boilerplade code taken from the Lecture04/Program.cs 
            var configuration = new ConfigurationBuilder().AddUserSecrets(typeof(Context).Assembly).Build();
            var connectionString = configuration.GetConnectionString("ConnectionString");

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .Property(e => e.Degree)
                .HasConversion(e => e.ToString(), e => Enum.Parse<Degree>(e));

            modelBuilder
                .Entity<Student>()
                .HasData(
                    new Student { Id = 1, Degree = Degree.Bachelor },
                    new Student { Id = 2, Degree = Degree.Master },
                    new Student { Id = 3, Degree = Degree.PhD },
                    new Student { Id = 4, Degree = Degree.Other }
                );
        }
    }
}
