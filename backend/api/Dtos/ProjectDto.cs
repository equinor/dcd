
using api.Models;

namespace api.Dtos
{

    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public Guid CommonLibraryId { get; set; }
        public string CommonLibraryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateTimeOffset CreateDate { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public ICollection<CaseDto> Cases { get; set; } = null!;

        public ICollection<ExplorationDto> Explorations { get; set; } = null!;
        public ICollection<SurfDto> Surfs { get; set; } = null!;
        public ICollection<SubstructureDto> Substructures { get; set; } = null!;
        public ICollection<TopsideDto> Topsides { get; set; } = null!;
        public ICollection<TransportDto> Transports { get; set; } = null!;
        public ICollection<DrainageStrategyDto> DrainageStrategies { get; set; } = null!;
        public ICollection<WellProjectDto> WellProjects { get; set; } = null!;
    }
}
