using api.Models.Interfaces;

namespace api.Models;

public class Exploration : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    public virtual ICollection<ExplorationWell> ExplorationWells { get; set; } = [];
}
