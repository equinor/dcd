using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class Case : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool ReferenceCase { get; set; }
    public bool Archived { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
    public DateTime DGADate { get; set; }
    public DateTime DGBDate { get; set; }
    public DateTime DGCDate { get; set; }
    public DateTime APBODate { get; set; }
    public DateTime BORDate { get; set; }
    public DateTime VPBODate { get; set; }
    public DateTime DG0Date { get; set; }
    public DateTime DG1Date { get; set; }
    public DateTime DG2Date { get; set; }
    public DateTime DG3Date { get; set; }
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

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    public Guid DrainageStrategyLink { get; set; }

    [ForeignKey("DrainageStrategyLink")]
    public virtual DrainageStrategy? DrainageStrategy { get; set; }

    public Guid WellProjectLink { get; set; }
    [ForeignKey("WellProjectLink")]
    public virtual WellProject? WellProject { get; set; }

    public Guid SurfLink { get; set; }
    [ForeignKey("SurfLink")]
    public virtual Surf? Surf { get; set; }

    public Guid SubstructureLink { get; set; }
    [ForeignKey("SubstructureLink")]
    public virtual Substructure? Substructure { get; set; }

    public Guid TopsideLink { get; set; }
    [ForeignKey("TopsideLink")]
    public virtual Topside? Topside { get; set; }

    public Guid TransportLink { get; set; }
    [ForeignKey("TransportLink")]
    public virtual Transport? Transport { get; set; }

    public Guid OnshorePowerSupplyLink { get; set; }
    [ForeignKey("OnshorePowerSupplyLink")]
    public virtual OnshorePowerSupply? OnshorePowerSupply { get; set; }

    public Guid ExplorationLink { get; set; }
    [ForeignKey("ExplorationLink")]
    public virtual Exploration? Exploration { get; set; }

    public virtual ICollection<Image> Images { get; set; } = [];

    public virtual ICollection<TimeSeriesProfile> TimeSeriesProfiles { get; set; } = [];

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

public enum ArtificialLift
{
    NoArtificialLift,
    GasLift,
    ElectricalSubmergedPumps,
    SubseaBoosterPumps
}

public enum ProductionStrategyOverview
{
    Depletion,
    WaterInjection,
    GasInjection,
    WAG,
    Mixed
}
