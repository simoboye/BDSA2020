using System;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BDSA2020.Models.Tests
{
    public class CompanyRepositoryTests : SqlLiteContext
    {
        private readonly ICompanyRepository repository;

        public CompanyRepositoryTests() : base()
        {
            repository = new CompanyRepository(Context);
        }

        [Fact]
        public async Task GetCompaniesAsync_returns_list_of_companies()
        {
            // Act
            var actual = await repository.GetCompaniesAsync();

            // Assert
            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public async Task GetCompany_returns_the_requested_comapny() 
        {   
            var id = new Guid("daccfa6a-6765-4295-82f1-49480ab2c2c1");
            var actual = await repository.GetCompanyAsync(id);

            var expected = new CompanyDetailsDTO
            { 
                PlacementDescriptionIds = new [] { 1, 2 },
                Id = id,
                Name = "Spotify"
            };

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.PlacementDescriptionIds.First(), actual.PlacementDescriptionIds.First());
        }

        [Fact]
        public async Task GetCompany_returns_ArgumentException_on_not_found_company() 
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetCompanyAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task CreateCompany_returns_the_id_of_created_company()
        {
            var id = Guid.NewGuid();
            var company = new CreateCompanyDTO
            { 
                Id = id,
                Name = "Test"
            };

            var actualId = await repository.CreateCompanyAsync(company);

            var createdCompany = await Context.Companies.FindAsync(actualId);
        
            Assert.Equal(company.Id, actualId);
            Assert.Equal(company.Name, createdCompany.Name);
        }

        [Fact]
        public async Task DeleteCompany_returns_true() 
        {
            var entity = await Context.Companies.FirstOrDefaultAsync();
            var actual = await repository.DeleteCompanyAsync(entity.Id);

            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteCompany_returns_ArgumentException_on_not_found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.DeleteCompanyAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateCompany_returns_true_on_updated()
        {
            var companyToUpdate = await Context.Companies.FirstAsync();
            var nameBeforeUpdate = companyToUpdate.Name;

            var dto = new UpdateCompanyDTO
            {
                Id = companyToUpdate.Id,
                Name = "Test"
            };
            var actual = await repository.UpdateCompanyAsync(dto);

            Assert.True(actual);
            Assert.NotEqual(nameBeforeUpdate, companyToUpdate.Name);
        }

        [Fact]
        public async Task UpdateCompany_returns_ArgumentException_on_not_found()
        {
            var companyToUpdate = new UpdateCompanyDTO { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => repository.UpdateCompanyAsync(companyToUpdate));
        }
    }
}
