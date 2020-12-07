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
            modelBuilder
                .Entity<Company>()
                .HasMany(company => company.PlacementDescriptions)
                .WithOne(description => description.Company);

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
        private Guid companyId2 = new Guid("a098132e-4636-4414-8ee8-ccc2405e3de4");
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
                new PlacementDescription { Id = 1, Degree = Degree.Other, MinSalary = 10, MinWorkingHours = 1, MaxWorkingHours = 100, Agreement = false, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 22), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://atb-tech.com/wp-content/uploads/2017/04/New-Tech-Gadgets-for-2017.jpg"), Title = "UML designer", Description = "You should be able to do UML diagrams correctly. Design a solid workflow and ensure quality within the company.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 2, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 23), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.groovypost.com/wp-content/uploads/2018/03/Microsoft_Sign_Featured.jpg"), Title = "C# developer", Description = "Join our team in of skilled developers.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 3, Degree = Degree.Master, MinSalary = 200, MinWorkingHours = 15, MaxWorkingHours = 100, Agreement = true, Location = "Aarhus", LastApplyDate = new DateTime(2020, 12, 21), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://static.adweek.com/adweek.com-prod/wp-content/uploads/2017/09/amazon-logo-CONTENT-2017-840x460.jpg"), Title = "Manager", Description = "We need someone to get things done.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 4, Degree = Degree.PhD, MinSalary = 229, MinWorkingHours = 10, MaxWorkingHours = 150, Agreement = true, Location = "Odense", LastApplyDate = new DateTime(2020, 12, 29), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.groovypost.com/wp-content/uploads/2018/03/Microsoft_Sign_Featured.jpg"), Title = "CEO", Description = "Let's make it happen!", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 5, Degree = Degree.Master, MinSalary = 279, MinWorkingHours = 20, MaxWorkingHours = 100, Agreement = true, Location = "Lyngby", LastApplyDate = new DateTime(2020, 12, 20), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.groovypost.com/wp-content/uploads/2018/03/Microsoft_Sign_Featured.jpg"), Title = "Web Developer", Description = "We need someone to design our webpage.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 6, Degree = Degree.Bachelor, MinSalary = 255, MinWorkingHours = 20, MaxWorkingHours = 150, Agreement = true, Location = "Frederikshavn", LastApplyDate = new DateTime(2020, 12, 27), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.finsmes.com/wp-content/uploads/2016/09/google.jpg"), Title = "Software Developer", Description = "We are looking for someone who knows his way around the delevopment of software.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 7, Degree = Degree.PhD, MinSalary = 110, MinWorkingHours = 10, MaxWorkingHours = 150, Agreement = true, Location = "Ballerup", LastApplyDate = new DateTime(2020, 12, 26), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://marketingland.com/wp-content/ml-loads/2014/08/google-building-sign-1920-800x450.jpg"), Title = "Front End Developer", Description = "Can you make breathtaking GUI's? Then you might be who we are looking for.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 8, Degree = Degree.Master, MinSalary = 579, MinWorkingHours = 5, MaxWorkingHours = 100, Agreement = true, Location = "Birkerød", LastApplyDate = new DateTime(2020, 12, 25), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.groovypost.com/wp-content/uploads/2018/03/Microsoft_Sign_Featured.jpg"), Title = "Network Engineer", Description = "Understand how to build a solid network? Then consider joining our team.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 9, Degree = Degree.Bachelor, MinSalary = 25, MinWorkingHours = 30, MaxWorkingHours = 150, Agreement = true, Location = "Fredensborg", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://atb-tech.com/wp-content/uploads/2017/04/New-Tech-Gadgets-for-2017.jpg"), Title = "Network Engineer", Description = "UDP, TCP and more - join our team today.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 10, Degree = Degree.Master, MinSalary = 250, MinWorkingHours = 20, MaxWorkingHours = 150, Agreement = true, Location = "Odense", LastApplyDate = new DateTime(2020, 12, 11), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://s1.ibtimes.com/sites/www.ibtimes.com/files/styles/embed/public/2019/02/13/apple-logo.jpg"), Title = "IOS Developer", Description = "Join the team leading the industry regarding deveopment of IOS.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 11, Degree = Degree.PhD, MinSalary = 120, MinWorkingHours = 10, MaxWorkingHours = 50, Agreement = true, Location = "Aalborg", LastApplyDate = new DateTime(2020, 12, 12), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://s1.ibtimes.com/sites/www.ibtimes.com/files/styles/embed/public/2019/02/13/apple-logo.jpg"), Title = "Junior Web", Description = "Can you build webpages using HTML and CSS? We are hiring a junior WebDev to help our competent team.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 12, Degree = Degree.Master, MinSalary = 129, MinWorkingHours = 20, MaxWorkingHours = 110, Agreement = true, Location = "Frederikshavn", LastApplyDate = new DateTime(2020, 12, 12), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.techexclusive.net/wp-content/uploads/2017/01/The-Future-Of-Apple.jpg"), Title = "Java Developer", Description = "Know your way around Java? Today is your chance to start changing the industry as we know it.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 13, Degree = Degree.Bachelor, MinSalary = 219, MinWorkingHours = 10, MaxWorkingHours = 70, Agreement = true, Location = "Næstved", LastApplyDate = new DateTime(2020, 12, 13), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.finsmes.com/wp-content/uploads/2016/09/google.jpg"), Title = "F# Developer", Description = "Functional programming is on demand, at our company the sky is the limit. Apply today.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 14, Degree = Degree.Master, MinSalary = 220, MinWorkingHours = 10, MaxWorkingHours = 60, Agreement = false, Location = "Aalborg", LastApplyDate = new DateTime(2020, 12, 14), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.techexclusive.net/wp-content/uploads/2017/01/The-Future-Of-Apple.jpg"), Title = "JavaScript", Description = "We need someone who can add additional functionality to our current webpages", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 15, Degree = Degree.PhD, MinSalary = 139, MinWorkingHours = 20, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 15), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.livetradingnews.com/wp-content/uploads/2017/07/AMD-Technology-3D-Logo-Amazing.jpg"), Title = "Communication", Description = "Help us achieve a better workflow around the office. Our team needs a competent person with the right mindset", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 16, Degree = Degree.Master, MinSalary = 229, MinWorkingHours = 10, MaxWorkingHours = 47, Agreement = true, Location = "Frederiksberg", LastApplyDate = new DateTime(2020, 12, 16), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://static.adweek.com/adweek.com-prod/wp-content/uploads/2017/09/amazon-logo-CONTENT-2017-840x460.jpg"), Title = "Communication", Description = "We need someone who can increase productivity around the office. You must be able to do solid UML.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 17, Degree = Degree.Bachelor, MinSalary = 140, MinWorkingHours = 20, MaxWorkingHours = 20, Agreement = true, Location = "Aarhus", LastApplyDate = new DateTime(2020, 12, 17), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://static.adweek.com/adweek.com-prod/wp-content/uploads/2017/09/amazon-logo-CONTENT-2017-840x460.jpg"), Title = "Communication", Description = "Help organize our team of 20 developers and ensure the right communication with our clients.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 18, Degree = Degree.Master, MinSalary = 15, MinWorkingHours = 10, MaxWorkingHours = 15, Agreement = true, Location = "Slangerup", LastApplyDate = new DateTime(2020, 12, 18), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://botw-pd.s3.amazonaws.com/logo_216.jpg"), Title = "FullStack", Description = "We are in need of someone with great potential, looking to learn more about the industry and improve his or her skillset.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 19, Degree = Degree.PhD, MinSalary = 10, MinWorkingHours = 20, MaxWorkingHours = 120, Agreement = true, Location = "Solrød", LastApplyDate = new DateTime(2020, 12, 19), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://selektope.com/wp-content/uploads/2017/08/I-Tech-logo-CMYK-300-dpi.jpg"), Title = "DevOps", Description = "Start developing new and revolutionary software solutions today.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 20, Degree = Degree.Master, MinSalary = 50, MinWorkingHours = 10, MaxWorkingHours = 110, Agreement = true, Location = "Køge", LastApplyDate = new DateTime(2020, 12, 9), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.groovypost.com/wp-content/uploads/2018/03/Microsoft_Sign_Featured.jpg"), Title = "Java", Description = "We are in need of a flexible Java Developer. By flexible we mean working around the clock.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 21, Degree = Degree.Bachelor, MinSalary = 60, MinWorkingHours = 20, MaxWorkingHours = 150, Agreement = true, Location = "Brovst", LastApplyDate = new DateTime(2020, 12, 9), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://marketingland.com/wp-content/ml-loads/2014/08/youtube-logo-name-1920.jpg"), Title = "DevOps", Description = "Join our team and let's create the best software solutions today.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 22, Degree = Degree.Master, MinSalary = 20, MinWorkingHours = 20, MaxWorkingHours = 140, Agreement = true, Location = "Albertslund", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://s3.amazonaws.com/cbi-research-portal-uploads/2019/09/04170610/tech-govt-feature.jpg"), Title = "FullStack", Description = "Know your way around a computer? We are in need of a part time FullStack developer.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 23, Degree = Degree.PhD, MinSalary = 130, MinWorkingHours = 20, MaxWorkingHours = 130, Agreement = true, Location = "Lyngby", LastApplyDate = new DateTime(2020, 12, 21), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://technative.io/wp-content/uploads/2017/03/AdobeStock_111309990.jpg"), Title = "UML", Description = "We need solid UML drawings to further the communication around our office.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 24, Degree = Degree.Master, MinSalary = 29, MinWorkingHours = 10, MaxWorkingHours = 120, Agreement = true, Location = "Lyngby", LastApplyDate = new DateTime(2020, 12, 22), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.riw.com/wp-content/uploads/2015/08/High-Tech-Materials.jpg"), Title = "DevOps", Description = "Help our company grow by implementing the right solutions for our clients.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 25, Degree = Degree.Bachelor, MinSalary = 22, MinWorkingHours = 15, MaxWorkingHours = 110, Agreement = true, Location = "Brøndby", LastApplyDate = new DateTime(2020, 12, 27), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://botw-pd.s3.amazonaws.com/logo_216.jpg"), Title = "FullStack", Description = "Our company offers a great working enviroment. Apply today if your have interest in growing your skillset.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 26, Degree = Degree.Master, MinSalary = 299, MinWorkingHours = 20, MaxWorkingHours = 100, Agreement = true, Location = "Taastrup", LastApplyDate = new DateTime(2020, 12, 23), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://i.ibb.co/StgTn0h/ascii.png"), Title = "C# developer", Description = "Apply today and get to work withing a competent team of C# developers", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 27, Degree = Degree.PhD, MinSalary = 300, MinWorkingHours = 10, MaxWorkingHours = 150, Agreement = true, Location = "Vejle", LastApplyDate = new DateTime(2020, 12, 25), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://rvptsa.digitalpto.com/files/2010/11/conseils-communication.jpg"), Title = "UML", Description = "Do you know how to build solid software. Our team needs someone that knows how to construct long lasting software solutions.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 28, Degree = Degree.Master, MinSalary = 200, MinWorkingHours = 10, MaxWorkingHours = 120, Agreement = true, Location = "Frederikshavn", LastApplyDate = new DateTime(2020, 12, 29), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://selektope.com/wp-content/uploads/2017/08/I-Tech-logo-CMYK-300-dpi.jpg"), Title = "C# Developer", Description = "Help our team develop some of the best software solutions with C#, Razor and Blazor.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 29, Degree = Degree.Other, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 50, Agreement = true, Location = "Frederiksberg", LastApplyDate = new DateTime(2020, 12, 30), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://fbcd.co/images/products/d85b5907a214411c0df814a958696bb7_resize.jpg"), Title = "C# Developer", Description = "We need someone with the right mindset to create new and interesting software implmentations.", CompanyId = GetCompanyData()[0].Id},
                new PlacementDescription { Id = 30, Degree = Degree.Other, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 60, Agreement = true, Location = "Frederiksberg", LastApplyDate = new DateTime(2020, 12, 30), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://www.groovypost.com/wp-content/uploads/2018/03/Microsoft_Sign_Featured.jpg"), Title = "Network Dev", Description = "We are looking for someone that can monitor our online traffic and enhance the customer flow.", CompanyId = GetCompanyData()[0].Id}
            }; 
        }


        private Guid id1 = new Guid("290c1a5f-3790-4bcb-89dc-6a4c3de155d1");
        private Guid id2 = Guid.NewGuid();
        private Guid id3 = Guid.NewGuid();
        private Guid id4 = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");

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
