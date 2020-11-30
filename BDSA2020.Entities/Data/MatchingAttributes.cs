using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2020.Entities
{
    public abstract class MatchingAttributes
    {
        [Required]
        public Degree Degree { get; set; }

        [Required]
        public int MinSalary { get; set; }

        [Required]
        public int MinWorkingHours { get; set; }

        [Required]
        public int MaxWorkingHours { get; set; }

        [Required]
        public bool Agreement { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
