using System;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Xunit;

namespace BDSA2020.View.Tests
{
    public class MatchingServiceTests
    {
        private readonly IMatchingService service;

        public MatchingServiceTests()
        {
            service = new MatchingService();
        }

        [Fact]
        public void Matches_returns_list_of_placement_descriptions()
        {
            var student = new StudentDetailsDTO
            {
                Id = Guid.NewGuid(),
                Degree = Degree.Bachelor,
                KeywordNames = new [] { "Java", "C#" },
                MinSalary = 10,
                MinWorkingHours = 10,
                MaxWorkingHours = 20,
                Agreement = true,
                Location = "Nørreport"
            };

            var placementDescriptions = new [] 
            {
                new PlacementDescriptionDetailsDTO { Id = 1, Degree = Degree.Other, KeywordNames = new [] { "Java", "C#" }, MinSalary = 10, MinWorkingHours = 1, MaxWorkingHours = 100, Agreement = false, Location = "Aalborg", LastApplyDate = new DateTime(2020, 12, 3), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "UML designer", Description = "You should be able to do UML diagrams correctly", CompanyName = "Test Company" },
                new PlacementDescriptionDetailsDTO { Id = 2, Degree = Degree.Bachelor, KeywordNames = new [] { "UML", "C#" }, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "C# developer", Description = "Join our team in of skilled developers", CompanyName = "Another Test Company" }
            };

            var actual = service.CalculateMatches(student, placementDescriptions);

            var expected = new [] 
            {
                new PlacementDescriptionMatch { Id = 2, MatchScore = 3.5f, Degree = Degree.Bachelor, KeywordNames = new [] { "UML", "C#" }, MinSalary = 100, MinWorkingHours = 10, MaxWorkingHours = 100, Agreement = true, Location = "Copenhagen", LastApplyDate = new DateTime(2020, 12, 10), Email = "ApplyHere@apply.com", Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), Title = "C# developer", Description = "Join our team in of skilled developers", CompanyName = "Another Test Company" }
            };

            Assert.Equal(expected.Count(), actual.Count);
            
            var actualArray = actual.ToArray();
            for (var i = 0; i < actualArray.Length; i++)
            {
                Assert.Equal(expected[i].MatchScore, actualArray[i].MatchScore);
            }
        }
    }
}