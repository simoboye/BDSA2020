using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2020.Entities
{
    public class Student : MatchingAttributes
    {
        public virtual ICollection<Saved> PlacementDescriptions { get; set; }
    }
}