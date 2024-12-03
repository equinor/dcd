using System.ComponentModel.DataAnnotations;

namespace api.Features.CaseProfiles.Dtos.Well;

public class DeleteWellDto
{
    [Required]
    public Guid Id { get; set; }
}
