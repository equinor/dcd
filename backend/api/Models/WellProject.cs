using api.Models.Interfaces;

namespace api.Models;

public class WellProject : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }

    public List<DevelopmentWell> DevelopmentWells { get; set; } = [];

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
