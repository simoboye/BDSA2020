using System.Collections.Generic;

namespace BDSA2020.Entities
{
    public class Student : MatchingAttributes
    {
        public virtual ICollection<Saved> PlacementDescriptions { get; set; }

        public virtual ICollection<StudentKeyword> Keywords { get; set; }
    }
}
