namespace api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTimeOffset CreateDate { get; set; }
        public ICollection<Case>? Cases { get; set; }
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

    public enum ProjectPhase
    {
        DG1,
        DG2,
        DG3,
        DG4
    }

    public enum ProjectCategory
    {
        OffshoreWind,
        Hydrogen,
        CarbonCaptureAndStorage,
        Solar,
        FPSO,
        Platform,
        TieIn,
        Electrification,
        Brownfield,
        Onshore,
        Pipeline,
        Subsea,
        DrillingUpgrade,
        Cessation,
    }
}
