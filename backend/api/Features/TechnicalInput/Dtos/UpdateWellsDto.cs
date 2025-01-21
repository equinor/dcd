using System.ComponentModel.DataAnnotations;

using api.Features.Wells.Create;
using api.Features.Wells.Update;

namespace api.Features.TechnicalInput.Dtos;

public class UpdateWellsDto
{
    [Required] public required List<UpdateWellDto> UpdateWellDtos { get; set; }
    [Required] public required List<CreateWellDto> CreateWellDtos { get; set; }
    [Required] public required List<DeleteWellDto> DeleteWellDtos { get; set; }
}
