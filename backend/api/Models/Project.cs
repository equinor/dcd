namespace api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
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
