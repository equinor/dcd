using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
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
    public bool Archived { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
    public DateTime? SharepointUpdatedTimestampUtc { get; set; }
    public DateTime? DgaDate { get; set; }
    public DateTime? DgbDate { get; set; }
    public DateTime? DgcDate { get; set; }
    public DateTime? ApboDate { get; set; }
    public DateTime? BorDate { get; set; }
    public DateTime? VpboDate { get; set; }
    public DateTime? Dg0Date { get; set; }
    public DateTime? Dg1Date { get; set; }
    public DateTime? Dg2Date { get; set; }
    public DateTime? Dg3Date { get; set; }
    public DateTime Dg4Date { get; set; }

    public int[] Co2EmissionsYears { get; set; } = new int[2];
    public int[] DrillingScheduleYears { get; set; } = new int[2];
    public int[] CaseCostYears { get; set; } = new int[2];
    public int[] ProductionProfilesYears { get; set; } = new int[2];

    public ArtificialLift ArtificialLift { get; set; }
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }

    /// <summary> Percentage </summary>
    public double FacilitiesAvailability { get; set; }

    public double CapexFactorFeasibilityStudies { get; set; }
    public double CapexFactorFeedStudies { get; set; }
    public double InitialYearsWithoutWellInterventionCost { get; set; }
    public double FinalYearsWithoutWellInterventionCost { get; set; }

    /// <summary> million USD </summary>
    public double Npv { get; set; }

    /// <summary> million USD </summary>
    public double? NpvOverride { get; set; }

    /// <summary> USD/bbl </summary>
    public double BreakEven { get; set; }

    /// <summary> USD/bbl </summary>
    public double? BreakEvenOverride { get; set; }

    public double Co2RemovedFromGas { get; set; }
    public double Co2EmissionFromFuelGas { get; set; }
    public double FlaredGasPerProducedVolume { get; set; }
    public double Co2EmissionsFromFlaredGas { get; set; }
    public double Co2Vented { get; set; }
    public double DailyEmissionFromDrillingRig { get; set; }
    public double AverageDevelopmentDrillingDays { get; set; }

    public string? Host { get; set; }
    public double AverageCo2Intensity { get; set; }
    public double DiscountedCashflow { get; set; }

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

    public TimeSeriesProfile? GetOverrideProfileOrProfile(string profileType)
    {
        var profileTypeOverride = $"{profileType}Override";
        var profileOverride = GetProfileOrNull(profileTypeOverride);

        return profileOverride?.Override == true ? profileOverride : GetProfileOrNull(profileType);
    }

    public TimeSeries GetProductionAndAdditionalProduction(string profileType)
    {
        var additionalProfileType = $"Additional{profileType}";

        var profile = new TimeSeries(GetProfileOrNull(profileType));
        var additionalProfile = new TimeSeries(GetProfileOrNull(additionalProfileType));

        var totalProfile = TimeSeriesMerger.MergeTimeSeries(profile, additionalProfile);

        return totalProfile;
    }
}
