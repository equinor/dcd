using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class DrainageStrategyMapper
{
    public static DrainageStrategyDto MapToDto(DrainageStrategy entity)
    {
        return new DrainageStrategyDto
        {
            Id = entity.Id,
            NglYield = entity.NglYield,
            CondensateYield = entity.CondensateYield,
            GasShrinkageFactor = entity.GasShrinkageFactor,
            ProducerCount = entity.ProducerCount,
            GasInjectorCount = entity.GasInjectorCount,
            WaterInjectorCount = entity.WaterInjectorCount,
            ArtificialLift = entity.ArtificialLift,
            GasSolution = entity.GasSolution
        };
    }
}
