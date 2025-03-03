using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class OnshorePowerSupplyMapper
{
    public static OnshorePowerSupplyDto MapToDto(OnshorePowerSupply entity)
    {
        return new OnshorePowerSupplyDto
        {
            Id = entity.Id,
            LastChangedDate = entity.UpdatedUtc,
            CostYear = entity.CostYear,
            Source = entity.Source,
            ProspVersion = entity.ProspVersion
        };
    }
}
