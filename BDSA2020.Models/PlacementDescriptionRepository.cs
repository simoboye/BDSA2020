using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
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

        public async Task<ICollection<PlacementDescription>> GetPlacementDescriptionsAsync()
        {
            var descriptions = context.PlacementDescriptions;
            return await descriptions.ToListAsync();
        }

        public async Task<PlacementDescription> GetPlacementDescriptionAsync(int id)
        {
            var description = await context.PlacementDescriptions.FindAsync(id);

            if (description == null)
            {
                throw new ArgumentException($"Could not find placement description with id {id}");
            }

            return description;
        }

        public async Task<int> CreatePlacementDescriptionAsync(PlacementDescription description)
        {
            var entity = await context.PlacementDescriptions.FindAsync(description.Id);

            if (entity != null)
            {
                throw new ArgumentException($"Placement description with id {description.Id} already exists");
            }

            await context.PlacementDescriptions.AddAsync(description);
            await context.SaveChangesAsync();

            return description.Id;
        }

        public async Task<bool> DeletePlacementDescriptionAsync(int id)
        {
            var description = await GetPlacementDescriptionAsync(id);

            context.PlacementDescriptions.Remove(description);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePlacementDescriptionAsync(PlacementDescription description)
        {
            var entity = await GetPlacementDescriptionAsync(description.Id);

            entity.Agreement = description.Agreement;
            entity.Degree = description.Degree;
            entity.Keywords = description.Keywords;
            entity.Location = description.Location;
            entity.MaxWorkingHours = description.MaxWorkingHours;
            entity.MinWorkingHours = description.MinWorkingHours;
            entity.MinSalary = description.MinSalary;
            entity.Students = description.Students;

            entity.Company = description.Company;
            entity.Description = description.Description;
            entity.Email = description.Email;
            entity.LastApplyDate = description.LastApplyDate;
            entity.Title = description.Title;
            entity.Thumbnail = description.Thumbnail;

            await context.SaveChangesAsync();

            return true;
        }
    }
}