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
            CO2ShareOilProfile = entity.CO2ShareOilProfile,
            CO2ShareGasProfile = entity.CO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = entity.CO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = entity.CO2OnMaxOilProfile,
            CO2OnMaxGasProfile = entity.CO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = entity.CO2OnMaxWaterInjectionProfile,
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
