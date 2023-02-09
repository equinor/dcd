using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class TopsideAdapter
{
    public static Topside Convert(TopsideDto topsideDto)
    {
        var topside = new Topside
        {
            Id = topsideDto.Id,
            Name = topsideDto.Name,
            ProjectId = topsideDto.ProjectId,
            DryWeight = topsideDto.DryWeight,
            OilCapacity = topsideDto.OilCapacity,
            GasCapacity = topsideDto.GasCapacity,
            WaterInjectionCapacity = topsideDto.WaterInjectionCapacity,
            ArtificialLift = topsideDto.ArtificialLift,
            Maturity = topsideDto.Maturity,
            Currency = topsideDto.Currency,
            FuelConsumption = topsideDto.FuelConsumption,
            FlaredGas = topsideDto.FlaredGas,
            ProducerCount = topsideDto.ProducerCount,
            GasInjectorCount = topsideDto.GasInjectorCount,
            WaterInjectorCount = topsideDto.WaterInjectorCount,
            CO2ShareOilProfile = topsideDto.CO2ShareOilProfile,
            CO2ShareGasProfile = topsideDto.CO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = topsideDto.CO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = topsideDto.CO2OnMaxOilProfile,
            CO2OnMaxGasProfile = topsideDto.CO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = topsideDto.CO2OnMaxWaterInjectionProfile,
            CostYear = topsideDto.CostYear,
            ProspVersion = topsideDto.ProspVersion,
            LastChangedDate = topsideDto.LastChangedDate,
            Source = topsideDto.Source,
            ApprovedBy = topsideDto.ApprovedBy,
            DG3Date = topsideDto.DG3Date,
            DG4Date = topsideDto.DG4Date,
            FacilityOpex = topsideDto.FacilityOpex,
            PeakElectricityImported = topsideDto.PeakElectricityImported,
        };

        if (topsideDto.CostProfile != null)
        {
            topside.CostProfile = Convert<TopsideCostProfileDto, TopsideCostProfile>(topsideDto.CostProfile, topside);
        }

        if (topsideDto.CostProfileOverride != null)
        {
            topside.CostProfileOverride = ConvertOverride<TopsideCostProfileOverrideDto, TopsideCostProfileOverride>(topsideDto.CostProfileOverride, topside);
        }

        if (topsideDto.CessationCostProfile != null)
        {
            topside.CessationCostProfile = Convert<TopsideCessationCostProfileDto, TopsideCessationCostProfile>(topsideDto.CessationCostProfile, topside);
        }

        return topside;
    }

    public static void ConvertExisting(Topside existing, TopsideDto topsideDto)
    {
        existing.Id = topsideDto.Id;
        existing.Name = topsideDto.Name;
        existing.ProjectId = topsideDto.ProjectId;
        existing.DryWeight = topsideDto.DryWeight;
        existing.OilCapacity = topsideDto.OilCapacity;
        existing.GasCapacity = topsideDto.GasCapacity;
        existing.WaterInjectionCapacity = topsideDto.WaterInjectionCapacity;
        existing.ArtificialLift = topsideDto.ArtificialLift;
        existing.Maturity = topsideDto.Maturity;
        existing.Currency = topsideDto.Currency;
        existing.CostProfile = Convert<TopsideCostProfileDto, TopsideCostProfile>(topsideDto.CostProfile, existing);
        existing.CostProfileOverride = ConvertOverride<TopsideCostProfileOverrideDto, TopsideCostProfileOverride>(topsideDto.CostProfileOverride, existing);
        existing.CessationCostProfile = Convert<TopsideCessationCostProfileDto, TopsideCessationCostProfile>(topsideDto.CessationCostProfile, existing);
        existing.FuelConsumption = topsideDto.FuelConsumption;
        existing.FlaredGas = topsideDto.FlaredGas;
        existing.ProducerCount = topsideDto.ProducerCount;
        existing.GasInjectorCount = topsideDto.GasInjectorCount;
        existing.WaterInjectorCount = topsideDto.WaterInjectorCount;
        existing.CO2ShareOilProfile = topsideDto.CO2ShareOilProfile;
        existing.CO2ShareGasProfile = topsideDto.CO2ShareGasProfile;
        existing.CO2ShareWaterInjectionProfile = topsideDto.CO2ShareWaterInjectionProfile;
        existing.CO2OnMaxOilProfile = topsideDto.CO2OnMaxOilProfile;
        existing.CO2OnMaxGasProfile = topsideDto.CO2OnMaxGasProfile;
        existing.CO2OnMaxWaterInjectionProfile = topsideDto.CO2OnMaxWaterInjectionProfile;
        existing.CostYear = topsideDto.CostYear;
        existing.ProspVersion = topsideDto.ProspVersion;
        existing.LastChangedDate = topsideDto.LastChangedDate;
        existing.Source = topsideDto.Source;
        existing.ApprovedBy = topsideDto.ApprovedBy;
        existing.DG3Date = topsideDto.DG3Date;
        existing.DG4Date = topsideDto.DG4Date;
        existing.FacilityOpex = topsideDto.FacilityOpex;
        existing.PeakElectricityImported = topsideDto.PeakElectricityImported;
    }

    private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, Topside topside)
        where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
        where TModel : TimeSeriesCost, ITimeSeriesOverride, ITopsideTimeSeries, new()
    {
        if (dto == null) { return new TModel(); }

        return new TModel
        {
            Id = dto.Id,
            Override = dto.Override,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Topside = topside,
        };
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, Topside topside)
        where TDto : TimeSeriesCostDto
        where TModel : TimeSeriesCost, ITopsideTimeSeries, new()
    {
        if (dto == null) { return new TModel(); }

        return new TModel
        {
            Id = dto.Id,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Topside = topside,
        };
    }
}
