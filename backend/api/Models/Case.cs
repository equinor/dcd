using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Case : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool ReferenceCase { get; set; }
    public bool Archived { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
    public DateTime? DGADate { get; set; }
    public DateTime? DGBDate { get; set; }
    public DateTime? DGCDate { get; set; }
    public DateTime? APBODate { get; set; }
    public DateTime? BORDate { get; set; }
    public DateTime? VPBODate { get; set; }
    public DateTime? DG0Date { get; set; }
    public DateTime? DG1Date { get; set; }
    public DateTime? DG2Date { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime DG4Date { get; set; }

    public ArtificialLift ArtificialLift { get; set; }
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double FacilitiesAvailability { get; set; }
    public double CapexFactorFeasibilityStudies { get; set; }
    public double CapexFactorFEEDStudies { get; set; }
    public double NPV { get; set; }
    public double? NPVOverride { get; set; }
    public double BreakEven { get; set; }
    public double? BreakEvenOverride { get; set; }
    public string? Host { get; set; }
    public double AverageCo2Intensity { get; set; }

    public Guid DrainageStrategyId { get; set; }
    public DrainageStrategy DrainageStrategy { get; set; } = null!;

    public Guid SurfId { get; set; }
    public Surf Surf { get; set; } = null!;

    public Guid SubstructureId { get; set; }
    public Substructure Substructure { get; set; } = null!;

    public Guid TopsideId { get; set; }
    public Topside Topside { get; set; } = null!;

    public Guid TransportId { get; set; }
    public Transport Transport { get; set; } = null!;

    public Guid OnshorePowerSupplyId { get; set; }
    public OnshorePowerSupply OnshorePowerSupply { get; set; } = null!;

    public Guid? OriginalCaseId { get; set; }
    public Case? OriginalCase { get; set; }

    public List<CaseImage> Images { get; set; } = [];
    public List<TimeSeriesProfile> TimeSeriesProfiles { get; set; } = [];
    public List<Campaign> Campaigns { get; set; } = [];
    public List<Case> RevisionCases { get; set; } = [];

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion

    public TimeSeriesProfile? GetProfileOrNull(string profileType) => TimeSeriesProfiles.SingleOrDefault(x => x.ProfileType == profileType);

    public TimeSeriesProfile GetProfile(string profileType) => TimeSeriesProfiles.Single(x => x.ProfileType == profileType);

    public TimeSeriesProfile CreateProfileIfNotExists(string profileType)
    {
        var profile = TimeSeriesProfiles.SingleOrDefault(x => x.ProfileType == profileType);

        if (profile != null)
        {
            return profile;
        }

        var newProfile = new TimeSeriesProfile { ProfileType = profileType, Values = [] };
        TimeSeriesProfiles.Add(newProfile);

        return newProfile;
    }
}
