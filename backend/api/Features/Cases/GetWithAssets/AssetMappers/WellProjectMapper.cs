using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class WellProjectMapper
{
    public static WellProjectDto MapToDto(WellProject entity)
    {
        return new WellProjectDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Name = entity.Name,
            ArtificialLift = entity.ArtificialLift,
            Currency = entity.Currency
        };
    }
}
