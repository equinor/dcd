namespace api.Models
{
    public class Well
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public WellTypeNew? WellType { get; set; }
        public ExplorationWellType? ExplorationWellType { get; set; }
        public double WellInterventionCost { get; set; }
        public double PlugingAndAbandonmentCost { get; set; }
    }
}
