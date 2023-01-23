using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class SurfDtoAdapter
{

    public static SurfDto Convert(Surf surf)
    {
        var surfDto = new SurfDto
        {
            Id = surf.Id,
            ProjectId = surf.ProjectId,
            Name = surf.Name,
            ArtificialLift = surf.ArtificialLift,
            Maturity = surf.Maturity,
            InfieldPipelineSystemLength = surf.InfieldPipelineSystemLength,
            UmbilicalSystemLength = surf.UmbilicalSystemLength,
            ProductionFlowline = surf.ProductionFlowline,
            RiserCount = surf.RiserCount,
            TemplateCount = surf.TemplateCount,
            ProducerCount = surf.ProducerCount,
            GasInjectorCount = surf.GasInjectorCount,
            WaterInjectorCount = surf.WaterInjectorCount,
            Currency = surf.Currency,

            LastChangedDate = surf.LastChangedDate,
            CostYear = surf.CostYear,
            Source = surf.Source,
            ProspVersion = surf.ProspVersion,
            ApprovedBy = surf.ApprovedBy,
            DG3Date = surf.DG3Date,
            DG4Date = surf.DG4Date,
            CessationCost = surf.CessationCost,
        };

        if (surf.CostProfile != null)
        {
            surfDto.CostProfile = Convert<SurfCostProfileDto, SurfCostProfile>(surf.CostProfile);
        }

        if (surf.CostProfileOverride != null)
        {
            surfDto.CostProfileOverride = ConvertOverride<SurfCostProfileOverrideDto, SurfCostProfileOverride>(surf.CostProfileOverride);
        }

        if (surf.CessationCostProfile != null)
        {
            surfDto.CessationCostProfile = Convert<SurfCessationCostProfileDto, SurfCessationCostProfile>(surf.CessationCostProfile);
        }

        return surfDto;
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
