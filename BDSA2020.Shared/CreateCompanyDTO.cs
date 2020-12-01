using System;
using System.Collections.Generic;
using BDSA2020.Entities;

namespace BDSA2020.Shared
{
    public class CreateCompanyDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public ISet<int> PlacementDescriptions { get; set; } = new HashSet<int>();
    }
}