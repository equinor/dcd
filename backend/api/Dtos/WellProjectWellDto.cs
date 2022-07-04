
using api.Models;

namespace api.Dtos
{
    public class WellProjectWellDto
    {
        public int Count { get; set; }
        public DrillingSchedule? DrillingSchedule { get; set; }
        public Guid WellProjectId { get; set; } = Guid.Empty!;
        public Guid WellId { get; set; } = Guid.Empty!;
    }
}
