using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Projects.Update;

public class UpdateProjectDto
{
    [Required] public required string Name { get; set; }
    public required Guid? ReferenceCaseId { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required string Country { get; set; }
    [Required] public required Currency Currency { get; set; }
    [Required] public required PhysUnit PhysicalUnit { get; set; }
    [Required] public required ProjectClassification Classification { get; set; }
    [Required] public required ProjectPhase ProjectPhase { get; set; }
    [Required] public required InternalProjectPhase InternalProjectPhase { get; set; }
    [Required] public required ProjectCategory ProjectCategory { get; set; }
    public required string? SharepointSiteUrl { get; set; }
    [Required] public required double OilPriceUsd { get; set; }
    [Required] public required double GasPriceNok { get; set; }
    [Required] public required double NglPriceUsd { get; set; }
    [Required] public required double DiscountRate { get; set; }
    [Required] public required double ExchangeRateUsdToNok { get; set; }
    [Required] public required int NpvYear { get; set; }
}
