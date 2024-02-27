using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class DeleteWellDto
{
    [Required]
    public Guid Id { get; set; }
}
