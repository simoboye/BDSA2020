using System.Threading.Tasks;
using System.Linq;
using Xunit;
using System;
using BDSA2020.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models.Tests
{
    public class PlacementDescriptionRepositoryTests : SqlLiteContext
    {
        private readonly IPlacementDescriptionRepository repository;

        public PlacementDescriptionRepositoryTests() : base()
        {
            repository = new PlacementDescriptionRepository(Context);
        }
        
        [Fact]
        public async Task GetPlacementDescriptions_returns_list_of_descriptions()
        {
            var actual = await repository.GetPlacementDescriptionsAsync();

            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public async Task GetPlacementDescription_returns_requested_description()
        {
            var expected = Context.PlacementDescriptions.First();

            var actual = await repository.GetPlacementDescriptionAsync(expected.Id);

            Assert.Equal(actual.Id, expected.Id);
        }

        [Fact]
        public async Task GetPlacementDescription_returns_ArgumentException_on_not_found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetPlacementDescriptionAsync(100));
        }

        [Fact]
        public async Task CreatePlacementDescription_returns_id_of_created()
        {
            var description = new PlacementDescription
            {
                Degree = Degree.Other, 
                MinSalary = 10, 
                MinWorkingHours = 1, 
                MaxWorkingHours = 100, 
                Agreement = false, 
                Location = "Copenhagen", 
                LastApplyDate = new DateTime(2020, 12, 3), 
                Email = "ApplyHere@apply.com", 
                Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), 
                Title = "UML designer", 
                Description = "You should be able to do UML diagrams correctly", 
                CompanyId = 1
            };

            var descriptionList = await Context.PlacementDescriptions.ToListAsync();
            var lastId = descriptionList.OrderByDescending(d => d.Id)
                                        .FirstOrDefault()
                                        .Id;

            var actual = await repository.CreatePlacementDescriptionAsync(description);

            Assert.Equal(lastId + 1, actual); 
        }

        [Fact]
        public async Task CreatePlacementDescription_returns_ArgumentException_on_conflict()
        {
            var description = new PlacementDescription
            {
                Id = 1,
                Degree = Degree.Other, 
                MinSalary = 10, 
                MinWorkingHours = 1, 
                MaxWorkingHours = 100, 
                Agreement = false, 
                Location = "Copenhagen", 
                LastApplyDate = new DateTime(2020, 12, 3), 
                Email = "ApplyHere@apply.com", 
                Thumbnail = new Uri("https://starwarsblog.starwars.com/wp-content/uploads/2020/04/best-friend-in-galaxy-chewbacca_TALL.jpg"), 
                Title = "UML designer", 
                Description = "You should be able to do UML diagrams correctly", 
                CompanyId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => repository.CreatePlacementDescriptionAsync(description));
        }

        [Fact]
        public async Task DeletePlacementDescription_returns_true() 
        {
            var actual = await repository.DeletePlacementDescriptionAsync(1);

            Assert.True(actual);
        }

        [Fact]
        public async Task DeletePlacementDescription_returns_ArgumentException_on_not_found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.DeletePlacementDescriptionAsync(100));
        }

        [Fact]
        public async Task UpdatePlacementDescription_returns_true_on_updated()
        {
            var descriptionToUpdate = await Context.PlacementDescriptions.FirstAsync();
            descriptionToUpdate.Degree = Degree.Other;
            var actual = await repository.UpdatePlacementDescriptionAsync(descriptionToUpdate);

            Assert.True(actual);
        }

        [Fact]
        public async Task UpdatePlacementDescription_returns_ArgumentException_on_not_found()
        {
            var descriptionToUpdate = new PlacementDescription { Id = 100 };
            await Assert.ThrowsAsync<ArgumentException>(() => repository.UpdatePlacementDescriptionAsync(descriptionToUpdate));
        }
    }
}