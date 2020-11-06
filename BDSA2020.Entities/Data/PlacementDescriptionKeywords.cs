namespace BDSA2020.Entities
{
    public class PlacementDescriptionKeywords
    {
        public int PlacementDescriptionId { get; set; }
        public virtual PlacementDescription PlacementDescription { get; set; }
        public int KeywordId { get; set; }
        public virtual string KeywordName { get; set; }
    }
}
