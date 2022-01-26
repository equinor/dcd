namespace api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTimeOffset CreateDate { get; set; }
        public virtual ICollection<Case>? Cases { get; set; }
        public virtual ICollection<Surf>? Surfs { get; set; }
        public virtual ICollection<Substructure>? Substructures { get; set; }
        public virtual ICollection<Topside>? Topsides { get; set; }
        public virtual ICollection<Transport>? Transports { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public virtual ICollection<DrainageStrategy>? DrainageStrategies { get; set; }
        public virtual ICollection<WellProject>? WellProjects { get; set; }
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
