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
            Name = entity.Name,
            ProjectId = entity.Case.ProjectId,
            LastChangedDate = entity.LastChangedDate,
            CostYear = entity.CostYear,
            Source = entity.Source,
            ProspVersion = entity.ProspVersion,
            DG3Date = entity.DG3Date,
            DG4Date = entity.DG4Date
        };
    }
}
