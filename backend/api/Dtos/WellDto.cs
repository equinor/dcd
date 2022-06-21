using api.Models;

namespace api.Dtos
{
    public class WellDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public WellTypeNew? WellType { get; set; }
        public double WellInterventionCost { get; set; }
        public double PlugingAndAbandonmentCost { get; set; }
    }
}
