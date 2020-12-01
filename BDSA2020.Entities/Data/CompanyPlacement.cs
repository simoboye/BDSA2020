using System;

namespace BDSA2020.Entities
{
    public class CompanyPlacement
    {
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int PlacementDescriptionId { get; set; }
        public PlacementDescription PlacementDescription { get; set; }
    }
}