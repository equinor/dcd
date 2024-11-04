using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CreateRevisionDto
{
    [Required]
    public string Name { get; set; } = null!;
    public InternalProjectPhase InternalProjectPhase { get; set; }
    public ProjectClassification Classification { get; set; }
}
