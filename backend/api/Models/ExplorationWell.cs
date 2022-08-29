
namespace api.Models
{
    public class ExplorationWell
    {
        public int Count { get; set; }
        public DrillingSchedule? DrillingSchedule { get; set; }
        public Exploration Exploration { get; set; } = null!;
        public Guid ExplorationId { get; set; } = Guid.Empty!;
        public Well Well { get; set; } = null!;
        public Guid WellId { get; set; } = Guid.Empty!;
    }
}
