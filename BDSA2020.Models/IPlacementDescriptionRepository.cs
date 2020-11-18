using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;

namespace BDSA2020.Models
{
    public interface IPlacementDescriptionRepository
    {
        Task<int> CreatePlacementDescriptionAsync(PlacementDescription description);
        Task<PlacementDescription> GetPlacementDescriptionAsync(int id);
        Task<ICollection<PlacementDescription>> GetPlacementDescriptionsAsync();
        Task<bool> UpdatePlacementDescriptionAsync(PlacementDescription description);
        Task<bool> DeletePlacementDescriptionAsync(int id);
    }
}