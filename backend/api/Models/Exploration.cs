using api.Models.Interfaces;

namespace api.Models;

public class Exploration : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public required string Name { get; set; }
    public required double RigMobDemob { get; set; }
    public required Currency Currency { get; set; }
    public required List<ExplorationWell> ExplorationWells { get; set; } = [];

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
