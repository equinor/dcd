using System.Drawing;

using api.Models;

using Microsoft.Identity.Client;

namespace api.Dtos
{

    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Country { get; set; } = null!;
        public ProjectPhase ProjectPhase { get; set; }
        public ProjectCategory ProjectCategory { get; set; }

    }
}
