using System.ComponentModel.DataAnnotations;

namespace api.Features.Wells.Update.Dtos;

public class DeleteWellDto
{
    [Required] public required Guid Id { get; set; }
}
