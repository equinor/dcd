using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class ExplorationMapper
{
    public static ExplorationDto MapToDto(Exploration entity)
    {
        return new ExplorationDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Name = entity.Name,
            RigMobDemob = entity.RigMobDemob,
            Currency = entity.Currency
        };
    }
}
