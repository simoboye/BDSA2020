namespace BDSA2020.Entities
{
    public class PlacementDescriptionKeyword
    {
        public int PlacementDescriptionId { get; set; }
        public PlacementDescription PlacementDescription { get; set; }
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
