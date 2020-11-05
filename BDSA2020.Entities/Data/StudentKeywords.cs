namespace BDSA2020.Entities
{
    public class StudentKeywords
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int KeywordId { get; set; }
        public string KeywordName { get; set; }
    }
}