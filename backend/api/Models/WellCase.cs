
namespace api.Models
{
    public class WellCase
    {
        public int Count { get; set; }
        public DrillingSchedule? DrillingSchedule { get; set; }
        public Case? Case { get; set; }
        public Well? Well { get; set; }
    }
}
