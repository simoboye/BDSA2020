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
            modelBuilder.Entity<Saved>().HasKey(e => new { e.StudentId, e.PlacementDescriptionId });

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

            // modelBuilder
            //     .Entity<PlacementDescription>()
            //     .HasOne(e => e.Company)
            //     .WithMany(e => e.PlacementDescriptions)
            //     .HasForeignKey(e => e.CompanyId);

            var descriptions = new[]
            {
                new PlacementDescription
                {
                    Id = 1,
                    Degree = Degree.Other,
                    Keywords = Keywords.UML,
                    MinSalary = 10,
                    MinWorkingHours = 1,
                    MaxWorkingHours = 100,
                    Agreement = false,
                    Location = "Copenhagen",
                    LastApplyDate = new DateTime(2020, 12, 3),
                    Email = "ApplyHere@apply.com",
                    Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"),
                    Title = "UML designer",
                    Description = "You should be able to do UML diagrams correctly"
                }
            };

            // modelBuilder
            //     .Entity<Company>()
            //     .HasData(
            //         new Company
            //         {
            //             Id = 1,
            //             Name = "UML-central",
            //             PlacementDescriptions = descriptions
            //         }
            //     );

            modelBuilder
                .Entity<PlacementDescription>()
                .HasData(
                    descriptions[0]
                );

            modelBuilder
                .Entity<Student>()
                .HasData(
                    new Student
                    {
                        Id = 1,
                        Degree = Degree.Bachelor,
                        Keywords = Keywords.BackEnd,
                        MinSalary = 100,
                        MinWorkingHours = 5,
                        MaxWorkingHours = 20,
                        Agreement = false,
                        Location = "Nowhere"
                    },
                    new Student
                    {
                        Id = 2,
                        Degree = Degree.Master,
                        Keywords = Keywords.FrontEnd,
                        MinSalary = 1000,
                        MinWorkingHours = 532,
                        MaxWorkingHours = 43243,
                        Agreement = false,
                        Location = "Anywhere"
                    },
                    new Student
                    {
                        Id = 3,
                        Degree = Degree.PhD,
                        Keywords = Keywords.Testing,
                        MinSalary = 10000,
                        MinWorkingHours = 5000,
                        MaxWorkingHours = 5001,
                        Agreement = true,
                        Location = "Glostrup"
                    },
                    new Student
                    {
                        Id = 4,
                        Degree = Degree.Other,
                        Keywords = Keywords.UML,
                        MinSalary = 1,
                        MinWorkingHours = 1,
                        MaxWorkingHours = 5,
                        Agreement = true,
                        Location = "Italy"
                    }
                );
        }
    }
}
