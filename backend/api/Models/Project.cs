
namespace api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTimeOffset CreateDate { get; set; }
        public virtual ICollection<Case> Cases { get; set; } = null!;
        public ProjectPhase ProjectPhase { get; set; }
    }

    public enum ProjectPhase
    {
        DG1,
        DG2,
        DG3,
        DG4
    }
}
