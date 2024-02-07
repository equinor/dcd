using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class SubstructureDtoAdapter
{
    public static SubstructureDto Convert(Substructure substructure)
    {
        var substructureDto = new SubstructureDto
        {
            Id = substructure.Id,
            ProjectId = substructure.ProjectId,
            Name = substructure.Name,
            DryWeight = substructure.DryWeight,
            Maturity = substructure.Maturity,
            Currency = substructure.Currency,
            ApprovedBy = substructure.ApprovedBy,
            CostYear = substructure.CostYear,
            CostProfile = Convert<SubstructureCostProfileDto, SubstructureCostProfile>(substructure.CostProfile) ?? new SubstructureCostProfileDto(),
            CostProfileOverride = ConvertOverride<SubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>(substructure.CostProfileOverride) ?? new SubstructureCostProfileOverrideDto(),
            CessationCostProfile = Convert<SubstructureCessationCostProfileDto, SubstructureCessationCostProfile>(substructure.CessationCostProfile) ?? new SubstructureCessationCostProfileDto(),
            ProspVersion = substructure.ProspVersion,
            Source = substructure.Source,
            LastChangedDate = substructure.LastChangedDate,
            Concept = substructure.Concept,
            DG3Date = substructure.DG3Date,
            DG4Date = substructure.DG4Date
        };
        return substructureDto;
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
