
using api.Models;

namespace api.Dtos
{

    public class CommonLibraryProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Country { get; set; } = null!;
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
    }
}
