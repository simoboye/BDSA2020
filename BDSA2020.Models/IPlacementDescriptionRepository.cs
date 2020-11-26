using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;

namespace BDSA2020.Models
{
    public interface IPlacementDescriptionRepository
    {
        Task<int> CreatePlacementDescriptionAsync(CreatePlacementDescriptionDTO description);
        Task<PlacementDescriptionDetailsDTO> GetPlacementDescriptionAsync(int id);
        Task<ICollection<PlacementDescriptionDetailsDTO>> GetPlacementDescriptionsAsync();
        Task<bool> UpdatePlacementDescriptionAsync(UpdatePlacementDescriptionDTO description);
        Task<bool> DeletePlacementDescriptionAsync(int id);
    }
}