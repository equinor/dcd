
using api.Models;

namespace api.Dtos
{
    public class WellCaseDto
    {
        public int Count { get; set; }
        public DrillingSchedule? DrillingSchedule { get; set; }
        public Guid CaseId { get; set; } = Guid.Empty!;
        public Guid WellId { get; set; } = Guid.Empty!;
    }
}
