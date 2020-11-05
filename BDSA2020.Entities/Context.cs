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
                .Entity<Student>()
                .Property(e => e.Keywords)
                .HasConversion(e => e.ToString(), e => Enum.Parse<Keywords>(e));

            modelBuilder
                .Entity<PlacementDescription>()
                .Property(e => e.Degree)
                .HasConversion(e => e.ToString(), e => Enum.Parse<Degree>(e));

            modelBuilder
                .Entity<PlacementDescription>()
                .Property(e => e.Keywords)
                .HasConversion(e => e.ToString(), e => Enum.Parse<Keywords>(e));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Saved>().HasKey(s => new { s.StudentId, s.PlacementDescriptionId });

            ParseEnums(modelBuilder);

            modelBuilder
                .Entity<Company>()
                .HasMany(e => e.PlacementDescriptions)
                .WithOne(e => e.Company);

            var company = new Company { Id = 1, Name = "UML-central" };

            var descriptions = new[]
            {
                new PlacementDescription { Id = 1, Degree = Degree.Other, Keywords = Keywords.UML, MinSalary = 10, MinWorkingHours = 1, MaxWorkingHours = 100, Agreement = false, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 3), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "UML designer", Description = "You should be able to do UML diagrams correctly", CompanyId = company.Id },
                new PlacementDescription { Id = 2, Degree = Degree.Bachelor, Keywords = Keywords.CSharp, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "C# developer", Description = "Join our team in of skilled developers", CompanyId = company.Id }
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
                    new Student { Id = 1, Degree = Degree.Bachelor, Keywords = Keywords.BackEnd, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 20, Agreement = false, Location = "Nowhere" },
                    new Student { Id = 2, Degree = Degree.Master, Keywords = Keywords.FrontEnd, MinSalary = 1000, MinWorkingHours = 532, MaxWorkingHours = 43243, Agreement = false, Location = "Anywhere" },
                    new Student { Id = 3, Degree = Degree.PhD, Keywords = Keywords.Testing, MinSalary = 10000, MinWorkingHours = 5000, MaxWorkingHours = 5001, Agreement = true, Location = "Glostrup" },
                    new Student { Id = 4, Degree = Degree.Other, Keywords = Keywords.UML, MinSalary = 1, MinWorkingHours = 1, MaxWorkingHours = 5, Agreement = true, Location = "Italy" }
                );

            modelBuilder
                .Entity<Saved>()
                .HasData(
                    new Saved { StudentId = 1, PlacementDescriptionId = 1 },
                    new Saved { StudentId = 1, PlacementDescriptionId = 2 },
                    new Saved { StudentId = 2, PlacementDescriptionId = 1 }
                );
        }
    }
}
