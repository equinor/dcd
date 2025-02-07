using api.Models.Interfaces;

namespace api.Models;

public class Project : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; } // If the project is a revision, this is the revision's id

    public Guid? OriginalProjectId { get; set; } // Id of the project the revision is based on
    public Project? OriginalProject { get; set; }

    public string Name { get; set; } = string.Empty;
    public bool IsRevision { get; set; }
    public Guid CommonLibraryId { get; set; }
    public Guid FusionProjectId { get; set; } // ExternalId
    public Guid ReferenceCaseId { get; set; }
    public string CommonLibraryName { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Currency Currency { get; set; }
    public PhysUnit PhysicalUnit { get; set; }
    public ProjectPhase ProjectPhase { get; set; }
    public InternalProjectPhase InternalProjectPhase { get; set; }
    public ProjectClassification Classification { get; set; }
    public ProjectCategory ProjectCategory { get; set; }
    public string? SharepointSiteUrl { get; set; }
    public double CO2RemovedFromGas { get; set; }
    public double CO2EmissionFromFuelGas { get; set; } = 2.34;
    public double FlaredGasPerProducedVolume { get; set; } = 1.122765;
    public double CO2EmissionsFromFlaredGas { get; set; } = 3.73;
    public double CO2Vented { get; set; } = 1.96;
    public double DailyEmissionFromDrillingRig { get; set; } = 100;
    public double AverageDevelopmentDrillingDays { get; set; } = 50;
    public double OilPriceUSD { get; set; }
    public double GasPriceNOK { get; set; }
    public double DiscountRate { get; set; }
    public double ExchangeRateUSDToNOK { get; set; }
    public int NpvYear { get; set; }

    public ExplorationOperationalWellCosts ExplorationOperationalWellCosts { get; set; } = null!;
    public DevelopmentOperationalWellCosts DevelopmentOperationalWellCosts { get; set; } = null!;
    public RevisionDetails? RevisionDetails { get; set; }

    public List<Case> Cases { get; set; } = [];
    public List<Well> Wells { get; set; } = [];
    public List<OnshorePowerSupply> OnshorePowerSupplies { get; set; } = [];
    public List<WellProject> WellProjects { get; set; } = [];
    public List<Exploration> Explorations { get; set; } = [];
    public List<Project> Revisions { get; set; } = [];
    public List<ProjectMember> ProjectMembers { get; set; } = [];
    public List<Image> Images { get; set; } = [];

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}

public enum PhysUnit
{
    SI,
    OilField,
}

public enum Currency
{
    NOK = 1,
    USD = 2,
}

public enum InternalProjectPhase
{
    APbo, // Approval Point Business Oppertunity
    BOR, // Business Opportunity Reconfirmation
    VPbo, // Valid Point Business Opportunity
}
public enum ProjectPhase
{
    Null,
    BidPreparations,
    BusinessIdentification,
    BusinessPlanning, // DG0
    ConceptPlanning, // DG1
    ConcessionNegotiations,
    Definition,
    Execution,
    Operation,
    ScreeningBusinessOpportunities,
}

public enum ProjectCategory
{
    Null,
    Brownfield,
    Cessation,
    DrillingUpgrade,
    Onshore,
    Pipeline,
    PlatformFpso,
    Subsea,
    Solar,
    Co2Storage,
    Efuel,
    Nuclear,
    Co2Capture,
    Fpso,
    Hydrogen,
    Hse,
    OffshoreWind,
    Platform,
    PowerFromShore,
    TieIn,
    RenewableOther,
    Ccs,
}

public enum ProjectClassification
{
    Open,
    Internal,
    Restricted,
    Confidential
}
