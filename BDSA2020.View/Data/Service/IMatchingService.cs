using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Shared;

namespace BDSA2020.View
{
    public interface IMatchingService
    {
        ICollection<PlacementDescriptionMatch> CalculateMatches(StudentDetailsDTO student, ICollection<PlacementDescriptionDetailsDTO> descriptions);
    }
}