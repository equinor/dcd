using System.ComponentModel.DataAnnotations;

namespace api.Features.TechnicalInput.Dtos;

public class DeleteWellDto
{
    [Required]
    public Guid Id { get; set; }
}
