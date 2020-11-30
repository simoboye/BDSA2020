using System;

namespace BDSA2020.Entities
{
    public class StudentKeyword
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
