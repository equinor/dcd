
namespace api.Models
{
    public class WellCase
    {
        public int Count { get; set; }
        public DrillingSchedule? DrillingSchedule { get; set; }
        public Case Case { get; set; } = null!;
        public Guid CaseId { get; set; } = Guid.Empty!;
        public Well Well { get; set; } = null!;
        public Guid WellId { get; set; } = Guid.Empty!;
    }
}
