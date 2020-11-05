using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace BDSA2020.Entities
{
    public class Context : DbContext, IContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<PlacementDescription> PlacementDescriptions { get; set; }
        public DbSet<Saved> Saved { get; set; }

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

        private void ParseEnums(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .Property(e => e.Degree)
                .HasConversion(e => e.ToString(), e => Enum.Parse<Degree>(e));

            modelBuilder
                .Entity<PlacementDescription>()
                .Property(e => e.Degree)
                .HasConversion(e => e.ToString(), e => Enum.Parse<Degree>(e));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Saved>().HasKey(s => new { s.StudentId, s.PlacementDescriptionId });
            modelBuilder.Entity<StudentKeywords>().HasKey(sk => new { sk.StudentId, sk.KeywordId });
            modelBuilder.Entity<PlacementDescriptionKeywords>().HasKey(pdk => new { pdk.PlacementDescriptionId, pdk.KeywordId });

            ParseEnums(modelBuilder);

            modelBuilder
                .Entity<Company>()
                .HasMany(e => e.PlacementDescriptions)
                .WithOne(e => e.Company);

            var company = new Company { Id = 1, Name = "UML-central" };

            var descriptions = new[]
            {
                new PlacementDescription { Id = 1, Degree = Degree.Other, MinSalary = 10, MinWorkingHours = 1, MaxWorkingHours = 100, Agreement = false, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 3), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "UML designer", Description = "You should be able to do UML diagrams correctly", CompanyId = company.Id },
                new PlacementDescription { Id = 2, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "C# developer", Description = "Join our team in of skilled developers", CompanyId = company.Id }
            };

            modelBuilder
                .Entity<Company>()
                .HasData(
                    company
                );

            modelBuilder
                .Entity<PlacementDescription>()
                .HasData(
                    descriptions
                );

            modelBuilder
                .Entity<Student>()
                .HasData(
                    new Student { Id = 1, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 20, Agreement = false, Location = "Nowhere" },
                    new Student { Id = 2, Degree = Degree.Master, MinSalary = 1000, MinWorkingHours = 532, MaxWorkingHours = 43243, Agreement = false, Location = "Anywhere" },
                    new Student { Id = 3, Degree = Degree.PhD, MinSalary = 10000, MinWorkingHours = 5000, MaxWorkingHours = 5001, Agreement = true, Location = "Glostrup" },
                    new Student { Id = 4, Degree = Degree.Other, MinSalary = 1, MinWorkingHours = 1, MaxWorkingHours = 5, Agreement = true, Location = "Italy" }
                );

            modelBuilder
                .Entity<Saved>()
                .HasData(
                    new Saved { StudentId = 1, PlacementDescriptionId = 1 },
                    new Saved { StudentId = 1, PlacementDescriptionId = 2 },
                    new Saved { StudentId = 2, PlacementDescriptionId = 1 }
                );
            
            var keywords = new []
            {
                new KeywordContainer { Id = 1, Name = "Testing" },
                new KeywordContainer { Id = 2, Name = "C#" },
                new KeywordContainer { Id = 3, Name = "Java" },
                new KeywordContainer { Id = 4, Name = "FullStack" },
                new KeywordContainer { Id = 5, Name = "Frontend" },
                new KeywordContainer { Id = 6, Name = "Backend" },
                new KeywordContainer { Id = 7, Name = "UML" },
                new KeywordContainer { Id = 8, Name = "DevOps" },
                new KeywordContainer { Id = 9, Name = "Communication" },
                new KeywordContainer { Id = 10, Name = "JavaScript" }
            };

            modelBuilder
                .Entity<KeywordContainer>()
                .HasData(
                    keywords
                );
            
            modelBuilder
                .Entity<StudentKeywords>()
                .HasData(
                    new StudentKeywords { StudentId = 1, KeywordId = 1 },
                    new StudentKeywords { StudentId = 1, KeywordId = 2 },
                    new StudentKeywords { StudentId = 2, KeywordId = 1 },
                    new StudentKeywords { StudentId = 3, KeywordId = 6 },
                    new StudentKeywords { StudentId = 4, KeywordId = 7 }
                );
            
            modelBuilder
                .Entity<PlacementDescriptionKeywords>()
                .HasData(
                    new PlacementDescriptionKeywords { PlacementDescriptionId = 1, KeywordId = 1 },
                    new PlacementDescriptionKeywords { PlacementDescriptionId = 1, KeywordId = 10 },
                    new PlacementDescriptionKeywords { PlacementDescriptionId = 2, KeywordId = 3 }
                );
        }
    }
}
