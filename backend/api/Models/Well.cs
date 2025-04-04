using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Well : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public required string? Name { get; set; }
    public required WellCategory WellCategory { get; set; }
    public required double WellCost { get; set; }
    public required double DrillingDays { get; set; }
    public required double PlugingAndAbandonmentCost { get; set; }
    public required double WellInterventionCost { get; set; }

    public List<CampaignWell> CampaignWells { get; set; } = [];

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion
}
