namespace BDSA2020.Entities
{
    public class Saved
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int PlacementDescriptionId { get; set; }
        public PlacementDescription PlacementDescription { get; set; }
    }
}
