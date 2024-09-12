namespace api.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CommonLibraryId { get; set; }
    public Guid FusionProjectId { get; set; }
    public Guid ReferenceCaseId { get; set; }
    public string CommonLibraryName { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Currency Currency { get; set; }
    public PhysUnit PhysicalUnit { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public virtual ICollection<Case>? Cases { get; set; }
    public virtual ICollection<Well>? Wells { get; set; }
    public virtual ICollection<Surf>? Surfs { get; set; }
    public virtual ICollection<Substructure>? Substructures { get; set; }
    public virtual ICollection<Topside>? Topsides { get; set; }
    public virtual ICollection<Transport>? Transports { get; set; }
    public ProjectPhase ProjectPhase { get; set; }
    public ProjectClassification Classification { get; set; }
    public ProjectCategory ProjectCategory { get; set; }
    public virtual ExplorationOperationalWellCosts? ExplorationOperationalWellCosts { get; set; }
    public virtual DevelopmentOperationalWellCosts? DevelopmentOperationalWellCosts { get; set; }
    public virtual ICollection<DrainageStrategy>? DrainageStrategies { get; set; }
    public virtual ICollection<WellProject>? WellProjects { get; set; }
    public virtual ICollection<Exploration>? Explorations { get; set; }
    public string? SharepointSiteUrl { get; set; }
    public double CO2RemovedFromGas { get; set; }
    public double CO2EmissionFromFuelGas { get; set; } = 2.34;
    public double FlaredGasPerProducedVolume { get; set; } = 1.13;
    public double CO2EmissionsFromFlaredGas { get; set; } = 3.74;
    public double CO2Vented { get; set; } = 1.96;
    public double DailyEmissionFromDrillingRig { get; set; } = 100;
    public double AverageDevelopmentDrillingDays { get; set; } = 50;
    public DateTimeOffset ModifyTime { get; set; } = DateTimeOffset.UtcNow;
    public double OilPrice { get; set; } = 75.0; // USD
    public double GasPrice { get; set; } = 3; // NOK
    public double DiscountRate { get; set; } = 8.0;

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
