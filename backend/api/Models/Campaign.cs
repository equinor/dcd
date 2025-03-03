using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Campaign : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public int RigUpgradingCostStartYear { get; set; }
    public string RigUpgradingCostInternalData { get; set; } = string.Empty;
    public int RigMobDemobCostStartYear { get; set; }
    public string RigMobDemobCostInternalData { get; set; } = string.Empty;
    public required CampaignType CampaignType { get; set; }
    public double RigUpgradingCost { get; set; }
    public double RigMobDemobCost { get; set; }

    public List<CampaignWell> CampaignWells { get; set; } = [];

    [NotMapped]
    public required double[] RigUpgradingCostValues
    {
        get => string.IsNullOrEmpty(RigUpgradingCostInternalData) ? [] : Array.ConvertAll(RigUpgradingCostInternalData.Split(';'), pf => (double)Convert.ChangeType(pf, typeof(double)));
        set => RigUpgradingCostInternalData = string.Join(";", value.Select(p => p.ToString()).ToArray());
    }

    [NotMapped]
    public required double[] RigMobDemobCostValues
    {
        get => string.IsNullOrEmpty(RigMobDemobCostInternalData) ? [] : Array.ConvertAll(RigMobDemobCostInternalData.Split(';'), pf => (double)Convert.ChangeType(pf, typeof(double)));
        set => RigMobDemobCostInternalData = string.Join(";", value.Select(p => p.ToString()).ToArray());
    }

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion
}
