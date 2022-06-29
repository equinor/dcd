namespace api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CommonLibraryId { get; set; }
        public Guid FusionProjectId { get; set; }
        public string CommonLibraryName { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public PhysUnit PhysicalUnit { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public ICollection<Case>? Cases { get; set; }
        public ICollection<Well>? Wells { get; set; }
        public ICollection<Surf>? Surfs { get; set; }
        public ICollection<Substructure>? Substructures { get; set; }
        public ICollection<Topside>? Topsides { get; set; }
        public ICollection<Transport>? Transports { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public ICollection<DrainageStrategy>? DrainageStrategies { get; set; }
        public ICollection<WellProject>? WellProjects { get; set; }
        public ICollection<Exploration>? Explorations { get; set; }
    }

    public enum PhysUnit
    {
        SI,
        OilField
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
        BusinessPlanning,
        ConceptPlanning,
        ConcessionNegotiations,
        Definition,
        Execution,
        Operation,
        ScreeningBusinessOpportunities
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
        Ccs
    }
}
