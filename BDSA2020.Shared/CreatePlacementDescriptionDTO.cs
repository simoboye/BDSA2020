using System;
using System.Collections.Generic;
using BDSA2020.Entities;

namespace BDSA2020.Shared
{
    public class CreatePlacementDescriptionDTO
    {
        public Degree Degree { get; set; }
        public ICollection<string> KeywordNames { get; set; }
        public int MinSalary { get; set; }
        public int MinWorkingHours { get; set; }
        public int MaxWorkingHours { get; set; }
        public bool Agreement { get; set; }
        public string Location { get; set; }
        public DateTime LastApplyDate { get; set; }
        public string Email { get; set; }
        public Uri Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
    }
}