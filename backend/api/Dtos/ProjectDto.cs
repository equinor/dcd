
using api.Models;

namespace api.Dtos
{

    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public Guid CommonLibraryId { get; set; }
        public Guid FusionProjectId { get; set; }
        public string CommonLibraryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Country { get; set; } = null!;
        public Currency Currency { get; set; }
        public PhysUnit PhysUnit { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public ICollection<CaseDto>? Cases { get; set; }
        public WellDto? Well { get; set; }
        public ICollection<ExplorationDto>? Explorations { get; set; }
        public ICollection<SurfDto>? Surfs { get; set; }
        public ICollection<SubstructureDto>? Substructures { get; set; }
        public ICollection<TopsideDto>? Topsides { get; set; }
        public ICollection<TransportDto>? Transports { get; set; }
        public ICollection<DrainageStrategyDto>? DrainageStrategies { get; set; }
        public ICollection<WellProjectDto>? WellProjects { get; set; }
    }
}
