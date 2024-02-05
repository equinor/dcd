using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class SubstructureAdapter
{
    public static Substructure Convert(SubstructureDto substructureDto)
    {
        var substructure = new Substructure
        {
            Id = substructureDto.Id,
            ProjectId = substructureDto.ProjectId,
            Name = substructureDto.Name,
            DryWeight = substructureDto.DryWeight,
            Maturity = substructureDto.Maturity,
            Currency = substructureDto.Currency,
            ApprovedBy = substructureDto.ApprovedBy,
            CostYear = substructureDto.CostYear,
            ProspVersion = substructureDto.ProspVersion,
            Source = substructureDto.Source,
            LastChangedDate = substructureDto.LastChangedDate,
            Concept = substructureDto.Concept,
            DG3Date = substructureDto.DG3Date,
            DG4Date = substructureDto.DG4Date
        };

        substructure.CostProfile = Convert<SubstructureCostProfileDto, SubstructureCostProfile>(substructureDto.CostProfile, substructure);
        substructure.CostProfileOverride = ConvertOverride<SubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>(substructureDto.CostProfileOverride, substructure);
        substructure.CessationCostProfile = Convert<SubstructureCessationCostProfileDto, SubstructureCessationCostProfile>(substructureDto.CessationCostProfile, substructure);

        return substructure;
    }

    public static void ConvertExisting(Substructure existing, SubstructureDto substructureDto)
    {
        existing.Id = substructureDto.Id;
        existing.ProjectId = substructureDto.ProjectId;
        existing.Name = substructureDto.Name;
        existing.DryWeight = substructureDto.DryWeight;
        existing.Maturity = substructureDto.Maturity;
        existing.Currency = substructureDto.Currency;
        existing.ApprovedBy = substructureDto.ApprovedBy;
        existing.CostYear = substructureDto.CostYear;
        existing.ProspVersion = substructureDto.ProspVersion;
        existing.Source = substructureDto.Source;
        existing.LastChangedDate = substructureDto.LastChangedDate;
        existing.Concept = substructureDto.Concept;
        existing.DG3Date = substructureDto.DG3Date;
        existing.DG4Date = substructureDto.DG4Date;

        existing.CostProfile = Convert<SubstructureCostProfileDto, SubstructureCostProfile>(substructureDto.CostProfile, existing);
        existing.CostProfileOverride = ConvertOverride<SubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>(substructureDto.CostProfileOverride, existing);
        existing.CessationCostProfile = Convert<SubstructureCessationCostProfileDto, SubstructureCessationCostProfile>(substructureDto.CessationCostProfile, existing);
    }

    private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, Substructure substructure)
        where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
        where TModel : TimeSeriesCost, ITimeSeriesOverride, ISubstructureTimeSeries, new()
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
            Substructure = substructure,
        };
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, Substructure substructure)
        where TDto : TimeSeriesCostDto
        where TModel : TimeSeriesCost, ISubstructureTimeSeries, new()
    {
        if (dto == null) { return new TModel(); }

        return new TModel
        {
            Id = dto.Id,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Substructure = substructure,
        };
    }
}
