using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BDSA2020.Entities
{
    public class Context : DbContext, IContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<PlacementDescription> PlacementDescriptions { get; set; }
        public DbSet<Saved> Saved { get; set; }
        public DbSet<CompanyPlacement> CompanyPlacements {get; set;}
        public DbSet<StudentKeyword> StudentKeywords { get; set; }
        public DbSet<PlacementDescriptionKeyword> PlacementDescriptionKeywords { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

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
            SetUpManyToManyKeys(modelBuilder);
            ParseEnums(modelBuilder);

            modelBuilder
                .Entity<Company>()
                .HasData(GetCompanyData());

            modelBuilder
                .Entity<PlacementDescription>()
                .HasData(GetDescriptionsData());

            modelBuilder
                .Entity<Student>()
                .HasData(GetStudentsData());

            modelBuilder
                .Entity<Saved>()
                .HasData(GetSavedData());

            modelBuilder
                .Entity<CompanyPlacement>()
                .HasData(GetCompanyPlacementsData());

            modelBuilder
                .Entity<Keyword>()
                .HasData(GetKeywordsData());
            
            modelBuilder
                .Entity<StudentKeyword>()
                .HasData(GetStudentKeywordsData());
            
            modelBuilder
                .Entity<PlacementDescriptionKeyword>()
                .HasData(GetPlacementDescriptionKeywordsData());
        }

        private void SetUpManyToManyKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Saved>().HasKey(s => new { s.StudentId, s.PlacementDescriptionId });
            modelBuilder.Entity<CompanyPlacement>().HasKey(st => new { st.CompanyId, st.PlacementDescriptionId });
            modelBuilder.Entity<StudentKeyword>().HasKey(sk => new { sk.StudentId, sk.KeywordId });
            modelBuilder.Entity<PlacementDescriptionKeyword>().HasKey(pdk => new { pdk.PlacementDescriptionId, pdk.KeywordId });
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

        private Guid companyId1 = new Guid("daccfa6a-6765-4295-82f1-49480ab2c2c1");
        private Guid companyId2 = Guid.NewGuid();
        private Company[] GetCompanyData()
        {
            return new []
            {
                new Company { Id = companyId1, Name = "Spotify"},
                new Company { Id = companyId2, Name = "The UML Company" },
            };
        }

        private ICollection<PlacementDescription> GetDescriptionsData()
        {
           return new[]
            {
                new PlacementDescription { Id = 1, Degree = Degree.Other, MinSalary = 10, MinWorkingHours = 1, MaxWorkingHours = 100, Agreement = false, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 3), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "UML designer", Description = "You should be able to do UML diagrams correctly"},
                new PlacementDescription { Id = 2, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "C# developer", Description = "Join our team in of skilled developers"}
            }; 
        }

        private Guid id1 = new Guid("290c1a5f-3790-4bcb-89dc-6a4c3de155d1");
        private Guid id2 = Guid.NewGuid();
        private Guid id3 = Guid.NewGuid();
        private Guid id4 = Guid.NewGuid();

        private Student[] GetStudentsData()
        {
            return new []
            {
                new Student { Id = id1, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 20, Agreement = false, Location = "Nowhere" },
                new Student { Id = id2, Degree = Degree.Master, MinSalary = 1000, MinWorkingHours = 532, MaxWorkingHours = 43243, Agreement = false, Location = "Anywhere" },
                new Student { Id = id3, Degree = Degree.PhD, MinSalary = 10000, MinWorkingHours = 5000, MaxWorkingHours = 5001, Agreement = true, Location = "Glostrup" },
                new Student { Id = id4, Degree = Degree.Other, MinSalary = 1, MinWorkingHours = 1, MaxWorkingHours = 5, Agreement = true, Location = "Italy" }
            };
        }

        private ICollection<Saved> GetSavedData()
        {
            return new []
            {
                new Saved { StudentId = GetStudentsData()[0].Id, PlacementDescriptionId = 1 },
                new Saved { StudentId = GetStudentsData()[0].Id, PlacementDescriptionId = 2 },
                new Saved { StudentId = GetStudentsData()[1].Id, PlacementDescriptionId = 1 }
            };
        } 

        private ICollection<CompanyPlacement> GetCompanyPlacementsData()
        {
            return new []
            {
                new CompanyPlacement { CompanyId = GetCompanyData()[0].Id, PlacementDescriptionId = 1 },
                new CompanyPlacement { CompanyId = GetCompanyData()[0].Id, PlacementDescriptionId = 2 },
                new CompanyPlacement { CompanyId = GetCompanyData()[1].Id, PlacementDescriptionId = 1 }
            };
        } 

        private ICollection<Keyword> GetKeywordsData()
        {
            return new []
            {
                new Keyword { Id = 1, Name = "Testing" },
                new Keyword { Id = 2, Name = "C#" },
                new Keyword { Id = 3, Name = "Java" },
                new Keyword { Id = 4, Name = "FullStack" },
                new Keyword { Id = 5, Name = "Frontend" },
                new Keyword { Id = 6, Name = "Backend" },
                new Keyword { Id = 7, Name = "UML" },
                new Keyword { Id = 8, Name = "DevOps" },
                new Keyword { Id = 9, Name = "Communication" },
                new Keyword { Id = 10, Name = "JavaScript" }
            };
        }

        private ICollection<StudentKeyword> GetStudentKeywordsData()
        {
            return new []
            {
                new StudentKeyword { StudentId = GetStudentsData()[0].Id, KeywordId = 1 },
                new StudentKeyword { StudentId = GetStudentsData()[0].Id, KeywordId = 2 },
                new StudentKeyword { StudentId = GetStudentsData()[1].Id, KeywordId = 1 },
                new StudentKeyword { StudentId = GetStudentsData()[2].Id, KeywordId = 6 },
                new StudentKeyword { StudentId = GetStudentsData()[3].Id, KeywordId = 7 }
            };
        }

        private ICollection<PlacementDescriptionKeyword> GetPlacementDescriptionKeywordsData()
        {
            return new []
            {
                new PlacementDescriptionKeyword { PlacementDescriptionId = 1, KeywordId = 1 },
                new PlacementDescriptionKeyword { PlacementDescriptionId = 1, KeywordId = 10 },
                new PlacementDescriptionKeyword { PlacementDescriptionId = 2, KeywordId = 3 }
            };
        }
    }
}
