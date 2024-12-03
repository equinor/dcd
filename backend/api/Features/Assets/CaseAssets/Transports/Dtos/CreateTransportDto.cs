using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Dtos;

public class CreateTransportDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Source Source { get; set; }
}
