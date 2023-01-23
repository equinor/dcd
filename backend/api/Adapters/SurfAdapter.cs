using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class SurfAdapter
{

    public static Surf Convert(SurfDto surfDto)
    {
        var surf = new Surf
        {
            Id = surfDto.Id,
            ProjectId = surfDto.ProjectId,
            Name = surfDto.Name,
            ArtificialLift = surfDto.ArtificialLift,
            Maturity = surfDto.Maturity,
            InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength,
            UmbilicalSystemLength = surfDto.UmbilicalSystemLength,
            ProductionFlowline = surfDto.ProductionFlowline,
            RiserCount = surfDto.RiserCount,
            TemplateCount = surfDto.TemplateCount,
            ProducerCount = surfDto.ProducerCount,
            GasInjectorCount = surfDto.GasInjectorCount,
            WaterInjectorCount = surfDto.WaterInjectorCount,
            Currency = surfDto.Currency,
            LastChangedDate = surfDto.LastChangedDate,
            CostYear = surfDto.CostYear,
            Source = surfDto.Source,
            ProspVersion = surfDto.ProspVersion,
            ApprovedBy = surfDto.ApprovedBy,
            DG3Date = surfDto.DG3Date,
            DG4Date = surfDto.DG4Date,
            CessationCost = surfDto.CessationCost,
        };

        surf.CostProfile = Convert<SurfCostProfileDto, SurfCostProfile>(surfDto.CostProfile, surf);
        surf.CostProfileOverride = ConvertOverride<SurfCostProfileOverrideDto, SurfCostProfileOverride>(surfDto.CostProfileOverride, surf);
        surf.CessationCostProfile = Convert<SurfCessationCostProfileDto, SurfCessationCostProfile>(surfDto.CessationCostProfile, surf);

        return surf;
    }

    public static void ConvertExisting(Surf existing, SurfDto surfDto)
    {
        existing.Id = surfDto.Id;
        existing.ProjectId = surfDto.ProjectId;
        existing.Name = surfDto.Name;
        existing.ArtificialLift = surfDto.ArtificialLift;
        existing.Maturity = surfDto.Maturity;
        existing.InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength;
        existing.UmbilicalSystemLength = surfDto.UmbilicalSystemLength;
        existing.ProductionFlowline = surfDto.ProductionFlowline;
        existing.RiserCount = surfDto.RiserCount;
        existing.TemplateCount = surfDto.TemplateCount;
        existing.ProducerCount = surfDto.ProducerCount;
        existing.GasInjectorCount = surfDto.GasInjectorCount;
        existing.WaterInjectorCount = surfDto.WaterInjectorCount;
        existing.Currency = surfDto.Currency;
        existing.CostProfile = Convert<SurfCostProfileDto, SurfCostProfile>(surfDto.CostProfile, existing);
        existing.CostProfileOverride = ConvertOverride<SurfCostProfileOverrideDto, SurfCostProfileOverride>(surfDto.CostProfileOverride, existing);
        existing.CessationCostProfile = Convert<SurfCessationCostProfileDto, SurfCessationCostProfile>(surfDto.CessationCostProfile, existing);
        existing.LastChangedDate = surfDto.LastChangedDate;
        existing.CostYear = surfDto.CostYear;
        existing.Source = surfDto.Source;
        existing.ProspVersion = surfDto.ProspVersion;
        existing.ApprovedBy = surfDto.ApprovedBy;
        existing.DG3Date = surfDto.DG3Date;
        existing.DG4Date = surfDto.DG4Date;
        existing.CessationCost = surfDto.CessationCost;
    }

    private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, Surf surf)
        where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
        where TModel : TimeSeriesCost, ITimeSeriesOverride, ISurfTimeSeries, new()
    {
        if (dto == null) { return null; }

        return new TModel
        {
            Id = dto.Id,
            Override = dto.Override,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Surf = surf,
        };
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, Surf surf)
        where TDto : TimeSeriesCostDto
        where TModel : TimeSeriesCost, ISurfTimeSeries, new()
    {
        if (dto == null) { return null; }

        return new TModel
        {
            Id = dto.Id,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Surf = surf,
        };
    }
}
