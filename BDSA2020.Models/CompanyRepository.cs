using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IContext context;

        public CompanyRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<CompanyDetailsDTO>> GetCompaniesAsync()
        {
            var companies = from c in context.Companies
                           select new CompanyDetailsDTO
                           {
                               Id = c.Id,
                               Name = c.Name,
                               PlacementDescriptionIds = c.PlacementDescriptions.Select(p => p.Id).ToList()
                           };

            return await companies.ToListAsync();
        }

        public async Task<CompanyDetailsDTO> GetCompanyAsync(Guid id)
        {
            var companyQuery = from c in context.Companies
                           where c.Id.Equals(id)
                           select new CompanyDetailsDTO
                           {
                               Id = c.Id,
                               Name = c.Name,
                               PlacementDescriptionIds = c.PlacementDescriptions.Select(p => p.Id).ToList()
                           };

            var company = await companyQuery.FirstOrDefaultAsync();

            if (company == null)
            {
                throw new ArgumentException($"Could not find company with id {id}.");
            }

            return company;
        }

        public async Task<Guid> CreateCompanyAsync(CreateCompanyDTO company)
        {
            var entity = new Company
            {
                Id = company.Id,
                Name = company.Name,
            };

            await context.Companies.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            var company = await context.Companies.FindAsync(id);

            if (company == null)
            {
                throw new ArgumentException($"Could not remove company with id {id}, because it does not exist.");
            }

            context.Companies.Remove(company);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCompanyAsync(UpdateCompanyDTO company)
        {
            var entity = await context.Companies.FindAsync(company.Id);

            if (entity == null)
            {
                throw new ArgumentException($"Could not remove company with id {company.Id}, because it does not exist.");
            }

            entity.Name = company.Name;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
