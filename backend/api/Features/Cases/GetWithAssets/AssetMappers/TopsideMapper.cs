using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class TopsideMapper
{
    public static TopsideDto MapToDto(Topside entity)
    {
        return new TopsideDto
        {
            Id = entity.Id,
            DryWeight = entity.DryWeight,
            OilCapacity = entity.OilCapacity,
            GasCapacity = entity.GasCapacity,
            WaterInjectionCapacity = entity.WaterInjectionCapacity,
            ArtificialLift = entity.ArtificialLift,
            Maturity = entity.Maturity,
            FuelConsumption = entity.FuelConsumption,
            FlaredGas = entity.FlaredGas,
            ProducerCount = entity.ProducerCount,
            GasInjectorCount = entity.GasInjectorCount,
            WaterInjectorCount = entity.WaterInjectorCount,
            Co2ShareOilProfile = entity.Co2ShareOilProfile,
            Co2ShareGasProfile = entity.Co2ShareGasProfile,
            Co2ShareWaterInjectionProfile = entity.Co2ShareWaterInjectionProfile,
            Co2OnMaxOilProfile = entity.Co2OnMaxOilProfile,
            Co2OnMaxGasProfile = entity.Co2OnMaxGasProfile,
            Co2OnMaxWaterInjectionProfile = entity.Co2OnMaxWaterInjectionProfile,
            CostYear = entity.CostYear,
            ProspVersion = entity.ProspVersion,
            LastChangedDate = entity.UpdatedUtc,
            Source = entity.Source,
            ApprovedBy = entity.ApprovedBy,
            FacilityOpex = entity.FacilityOpex,
            PeakElectricityImported = entity.PeakElectricityImported
        };
    }
}
