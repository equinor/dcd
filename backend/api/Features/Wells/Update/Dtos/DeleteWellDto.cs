using System.ComponentModel.DataAnnotations;

namespace api.Features.Wells.Update.Dtos;

public class DeleteWellDto
{
    [Required] public Guid Id { get; set; }
}
