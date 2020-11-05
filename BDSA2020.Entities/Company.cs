using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2020.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<PlacementDescription> PlacementDescriptions { get; set; }
    }
}