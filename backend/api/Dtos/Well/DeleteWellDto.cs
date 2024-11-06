using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class DeleteWellDto
{
    [Required]
    public Guid Id { get; set; }
}
