using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models
{
    public class PlacementDescriptionRepository : IPlacementDescriptionRepository
    {
        private readonly IContext context;

        public PlacementDescriptionRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<PlacementDescriptionDetailsDTO>> GetPlacementDescriptionsAsync()
        {
            var descriptions = from d in context.PlacementDescriptions
                               select new PlacementDescriptionDetailsDTO
                               {
                                    Id = d.Id,
                                    Degree = d.Degree,
                                    KeywordNames = d.Keywords.Select(k => k.Keyword.Name).ToList(),
                                    MinSalary = d.MinSalary,
                                    MinWorkingHours = d.MinWorkingHours,
                                    MaxWorkingHours = d.MaxWorkingHours,
                                    Agreement = d.Agreement,
                                    Location = d.Location,
                                    LastApplyDate = d.LastApplyDate,
                                    Email = d.Email,
                                    Thumbnail = d.Thumbnail,
                                    Title = d.Title,
                                    Description = d.Description,
                                    CompanyName = d.Company.Name,
                                    NumberOfStudentsSaved = d.Students.Select(s => s.Student).Count()
                               };
            return await descriptions.ToListAsync();
        }

        public async Task<PlacementDescriptionDetailsDTO> GetPlacementDescriptionAsync(int id)
        {
            var descriptionQuery =  from d in context.PlacementDescriptions
                                    where d.Id == id
                                    select new PlacementDescriptionDetailsDTO
                                    {
                                        Id = d.Id,
                                        Degree = d.Degree,
                                        KeywordNames = d.Keywords.Select(k => k.Keyword.Name).ToList(),
                                        MinSalary = d.MinSalary,
                                        MinWorkingHours = d.MinWorkingHours,
                                        MaxWorkingHours = d.MaxWorkingHours,
                                        Agreement = d.Agreement,
                                        Location = d.Location,
                                        LastApplyDate = d.LastApplyDate,
                                        Email = d.Email,
                                        Thumbnail = d.Thumbnail,
                                        Title = d.Title,
                                        Description = d.Description,
                                        CompanyName = d.Company.Name,
                                        NumberOfStudentsSaved = d.Students.Select(s => s.Student).Count()
                                    };

            var description = await descriptionQuery.FirstOrDefaultAsync();

            if (description == null)
            {
                throw new ArgumentException($"Could not find placement description with id {id}");
            }

            return description;
        }

        public async Task<int> CreatePlacementDescriptionAsync(CreatePlacementDescriptionDTO description)
        {
            var entity = new PlacementDescription
            {
                Degree = description.Degree,
                Keywords = GetKeywords(description.KeywordNames).ToList(),
                MinSalary = description.MinSalary,
                MinWorkingHours = description.MinWorkingHours,
                MaxWorkingHours = description.MaxWorkingHours,
                Agreement = description.Agreement,
                Location = description.Location,
                LastApplyDate = description.LastApplyDate,
                Email = description.Email,
                Thumbnail = description.Thumbnail,
                Title = description.Title,
                Description = description.Description,
                Company = await GetCompany(description.CompanyName)
            };

            await context.PlacementDescriptions.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> DeletePlacementDescriptionAsync(int id)
        {
            var description = await context.PlacementDescriptions.FindAsync(id);

            if (description == null)
            {
                throw new ArgumentException($"Could not remove placement description with id {id}, because it does not exist.");
            }

            context.PlacementDescriptions.Remove(description);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePlacementDescriptionAsync(UpdatePlacementDescriptionDTO description)
        {
            var entity = await context.PlacementDescriptions.FindAsync(description.Id);

            if (entity == null)
            {
                throw new ArgumentException($"Could not remove placement description with id {description.Id}, because it does not exist.");
            }

            // Remove the previous keywords, as they are a primary key
            var keywordsForDescription = from k in context.PlacementDescriptionKeywords
                                         where k.PlacementDescriptionId == entity.Id
                                         select k;

            context.PlacementDescriptionKeywords.RemoveRange(keywordsForDescription);

            entity.Agreement = description.Agreement;
            entity.Degree = description.Degree;
            entity.Keywords = GetKeywords(description.KeywordNames).ToList();
            entity.Location = description.Location;
            entity.MaxWorkingHours = description.MaxWorkingHours;
            entity.MinWorkingHours = description.MinWorkingHours;
            entity.MinSalary = description.MinSalary;
            entity.Company = await GetCompany(description.CompanyName);
            entity.Description = description.Description;
            entity.Email = description.Email;
            entity.LastApplyDate = description.LastApplyDate;
            entity.Title = description.Title;
            entity.Thumbnail = description.Thumbnail;

            await context.SaveChangesAsync();

            return true;
        }

        private IEnumerable<PlacementDescriptionKeyword> GetKeywords(IEnumerable<string> keywordNames) {
            var keywords = from k in context.Keywords
                           where keywordNames.Contains(k.Name)
                           select k;

            var keywordMap = keywords.ToDictionary(k => k.Name);

            foreach (var name in keywordNames)
            {
                if (keywordMap.TryGetValue(name, out Keyword keyword))
                {
                    yield return new PlacementDescriptionKeyword { Keyword = keyword };
                }
                else
                {
                    throw new ArgumentException($"Keyword with name {name} does not exit.");
                }
            }
        }

        private async Task<Company> GetCompany(string companyName)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Name.Equals(companyName));

            if (company == null)
            {
                throw new ArgumentException($"Company with name {companyName} does not exit.");
            }
            return company;
        }
    }
}