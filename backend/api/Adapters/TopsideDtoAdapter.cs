using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class TopsideDtoAdapter
{
    public static TopsideDto Convert(Topside topside)
    {
        var topsideDto = new TopsideDto
        {
            Id = topside.Id,
            Name = topside.Name,
            ProjectId = topside.ProjectId,
            DryWeight = topside.DryWeight,
            OilCapacity = topside.OilCapacity,
            GasCapacity = topside.GasCapacity,
            WaterInjectionCapacity = topside.WaterInjectionCapacity,
            ArtificialLift = topside.ArtificialLift,
            Maturity = topside.Maturity,
            Currency = topside.Currency,
            CostProfile = Convert<TopsideCostProfileDto, TopsideCostProfile>(topside.CostProfile) ?? new TopsideCostProfileDto(),
            CostProfileOverride = ConvertOverride<TopsideCostProfileOverrideDto, TopsideCostProfileOverride>(topside.CostProfileOverride) ?? new TopsideCostProfileOverrideDto(),
            CessationCostProfile = Convert<TopsideCessationCostProfileDto, TopsideCessationCostProfile>(topside.CessationCostProfile) ?? new TopsideCessationCostProfileDto(),
            FuelConsumption = topside.FuelConsumption,
            FlaredGas = topside.FlaredGas,
            ProducerCount = topside.ProducerCount,
            GasInjectorCount = topside.GasInjectorCount,
            WaterInjectorCount = topside.WaterInjectorCount,
            CO2ShareOilProfile = topside.CO2ShareOilProfile,
            CO2ShareGasProfile = topside.CO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = topside.CO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = topside.CO2OnMaxOilProfile,
            CO2OnMaxGasProfile = topside.CO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = topside.CO2OnMaxWaterInjectionProfile,
            CostYear = topside.CostYear,
            ProspVersion = topside.ProspVersion,
            LastChangedDate = topside.LastChangedDate,
            Source = topside.Source,
            ApprovedBy = topside.ApprovedBy,
            DG3Date = topside.DG3Date,
            DG4Date = topside.DG4Date,
            FacilityOpex = topside.FacilityOpex,
            PeakElectricityImported = topside.PeakElectricityImported,
        };
        return topsideDto;
    }

    public static TDto? Convert<TDto, TModel>(TModel? model)
        where TDto : TimeSeriesCostDto, new()
        where TModel : TimeSeriesCost
    {
        if (model == null) { return null; }

        return new TDto
        {
            Id = model.Id,
            Currency = model.Currency,
            EPAVersion = model.EPAVersion,
            Values = model.Values,
            StartYear = model.StartYear,
        };
    }

    public static TDto? ConvertOverride<TDto, TModel>(TModel? model)
        where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto, new()
        where TModel : TimeSeriesCost, ITimeSeriesOverride
    {
        if (model == null) { return null; }

        return new TDto
        {
            Id = model.Id,
            Override = model.Override,
            Currency = model.Currency,
            EPAVersion = model.EPAVersion,
            Values = model.Values,
            StartYear = model.StartYear,
        };
    }
}
