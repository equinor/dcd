using api.Models;

namespace api.Dtos
{
    public class WellDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public double WellInterventionCost { get; set; }
        public double PlugingAndAbandonmentCost { get; set; }
        public WellCategory WellCategory { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
    }
}
