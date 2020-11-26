namespace BDSA2020.Entities
{
    public class StudentKeyword
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
