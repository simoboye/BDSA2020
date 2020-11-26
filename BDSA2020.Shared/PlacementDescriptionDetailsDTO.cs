using System.Collections.Generic;
using BDSA2020.Entities;

namespace BDSA2020.Shared
{
    public class PlacementDescriptionDetailsDTO : CreatePlacementDescriptionDTO
    {
        public int Id { get; set; }
        public int NumberOfStudentsSaved { get; set; }
    }
}