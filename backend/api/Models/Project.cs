namespace api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTimeOffset CreateDate { get; set; }
        public ICollection<Case> Cases { get; set; } = null!;
        public ICollection<Surf> Surfs { get; set; } = null!;
        public ICollection<Substructure> Substructures { get; set; } = null!;
        public ICollection<Topside> Topsides { get; set; } = null!;
        public ICollection<Transport> Transports { get; set; } = null!;
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public ICollection<DrainageStrategy> DrainageStrategies { get; set; } = null!;
        public ICollection<WellProject> WellProjects { get; set; } = null!;
        public ICollection<Exploration> Explorations { get; set; } = null!;
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
