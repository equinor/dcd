using api.Models;

using Microsoft.Identity.Client;

namespace api.Dtos
{

    public class CaseDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Boolean ReferenceCase { get; set; }

        public DateTimeOffset DG4Date { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset ModifyTime { get; set; }
    }
}
