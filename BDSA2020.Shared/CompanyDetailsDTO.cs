using System.Collections.Generic;
using BDSA2020.Entities;

namespace BDSA2020.Shared
{
    public class CompanyDetailsDTO : CreateCompanyDTO
    {
        public ICollection<int> PlacementDescriptionIds { get; set; } = new HashSet<int>();
    }
}