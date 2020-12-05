using System.Linq;
using System.Collections.Generic;
using BDSA2020.Shared;

namespace BDSA2020.View
{
    public class MatchingService : IMatchingService
    {
        public MatchingService()
        {

        }

        public ICollection<PlacementDescriptionMatch> CalculateMatches(
            StudentDetailsDTO student, 
            ICollection<PlacementDescriptionDetailsDTO> descriptions)
        {
            var sameDegree = descriptions.Where(d => d.Degree == student.Degree);
            
            var matches = new List<PlacementDescriptionMatch>();
            foreach(var description in sameDegree)
            {
                matches.Add(CalculateScore(student, description));
            }

            var sorted = matches.OrderByDescending(m => m.MatchScore);

            return sorted.ToList();
        }

        private PlacementDescriptionMatch CalculateScore(StudentDetailsDTO student, PlacementDescriptionDetailsDTO description)
        {
            float score = 0;
            score += (float) student.KeywordNames.Where(k => description.KeywordNames.Contains(k)).Count() / 2f;
            score += student.MinSalary >= description.MinSalary ? 1 : 0;
            score += student.MinWorkingHours >= description.MinWorkingHours ? 1 : 0;
            score += student.MaxWorkingHours <= description.MaxWorkingHours ? 1 : 0;
            score += student.Agreement == description.Agreement ? 1 : 0;
            score += student.Location.Equals(description.Location) ? 1 : 0; // Primitive location matching

            return new PlacementDescriptionMatch
            {
                MatchScore = score, 
                Id = description.Id,
                Degree = description.Degree, 
                KeywordNames = description.KeywordNames, 
                MinSalary = description.MinSalary, 
                MinWorkingHours = description.MinWorkingHours,
                MaxWorkingHours = description.MaxWorkingHours, 
                Agreement = description.Agreement, 
                Location = description.Location, 
                LastApplyDate = description.LastApplyDate, 
                Email = description.Email, 
                Thumbnail = description.Thumbnail,
                Title = description.Title, 
                Description = description.Description,
                CompanyName = description.CompanyName
            };
        }
    }
}