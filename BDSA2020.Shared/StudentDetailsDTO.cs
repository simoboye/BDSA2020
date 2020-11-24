using System.Collections.Generic;
using BDSA2020.Entities;

namespace BDSA2020.Shared
{
    public class StudentDetailsDTO : CreateStudentDTO
    {
        public int Id { get; set; }
        public ICollection<PlacementDescription> PlacementDescriptions { get; set; }
    }
}