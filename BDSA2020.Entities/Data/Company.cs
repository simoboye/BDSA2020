using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2020.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // This list should also not be set, as the framework
        // is smart enough to use the JOIN and link it 
        // with the FK in 'PlacementDescription' that is 'CompanyId'
        public virtual ICollection<PlacementDescription> PlacementDescriptions { get; set; }
    }
}