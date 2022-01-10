
namespace api.Models
{

    public class Case
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public string Description { get; set; } = string.Empty!;
        public virtual Project Project { get; set; } = null!;
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
}
