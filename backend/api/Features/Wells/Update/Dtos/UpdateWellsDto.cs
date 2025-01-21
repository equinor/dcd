using System.ComponentModel.DataAnnotations;

namespace api.Features.Wells.Update.Dtos;

public class UpdateWellsDto
{
    [Required] public required List<UpdateWellDto> UpdateWellDtos { get; set; }
    [Required] public required List<CreateWellDto> CreateWellDtos { get; set; }
    [Required] public required List<DeleteWellDto> DeleteWellDtos { get; set; }
}
