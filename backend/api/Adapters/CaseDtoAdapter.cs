using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class CaseDtoAdapter
{
    public static CaseDto Convert(Case case_)
    {
        var caseDto = new CaseDto
        {
            Id = case_.Id,
            ProjectId = case_.ProjectId,
            Name = case_.Name,
            Description = case_.Description,
            ReferenceCase = case_.ReferenceCase,

            DGADate = case_.DGADate,
            DGBDate = case_.DGBDate,
            DGCDate = case_.DGCDate,
            APXDate = case_.APXDate,
            APZDate = case_.APZDate,
            DG0Date = case_.DG0Date,
            DG1Date = case_.DG1Date,
            DG2Date = case_.DG2Date,
            DG3Date = case_.DG3Date,
            DG4Date = case_.DG4Date,
            CreateTime = case_.CreateTime,
            ModifyTime = case_.ModifyTime,

            DrainageStrategyLink = case_.DrainageStrategyLink,
            WellProjectLink = case_.WellProjectLink,
            SurfLink = case_.SurfLink,
            SubstructureLink = case_.SubstructureLink,
            TopsideLink = case_.TopsideLink,
            TransportLink = case_.TransportLink,
            ExplorationLink = case_.ExplorationLink,

            ArtificialLift = case_.ArtificialLift,
            ProductionStrategyOverview = case_.ProductionStrategyOverview,
            ProducerCount = case_.ProducerCount,
            GasInjectorCount = case_.GasInjectorCount,
            WaterInjectorCount = case_.WaterInjectorCount,
            FacilitiesAvailability = case_.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = case_.CapexFactorFeasibilityStudies,
            CapexFactorFEEDStudies = case_.CapexFactorFEEDStudies,
            NPV = case_.NPV,
            BreakEven = case_.BreakEven,
            Host = case_.Host,

            SharepointFileId = case_.SharepointFileId,
            SharepointFileName = case_.SharepointFileName,
            SharepointFileUrl = case_.SharepointFileUrl,

            TotalFeasibilityAndConceptStudies = Convert<TotalFeasibilityAndConceptStudiesDto, TotalFeasibilityAndConceptStudies>(case_.TotalFeasibilityAndConceptStudies),
            TotalFeasibilityAndConceptStudiesOverride = ConvertOverride<TotalFeasibilityAndConceptStudiesOverrideDto,
                TotalFeasibilityAndConceptStudiesOverride>(case_.TotalFeasibilityAndConceptStudiesOverride),

            TotalFEEDStudies = Convert<TotalFEEDStudiesDto, TotalFEEDStudies>(case_.TotalFEEDStudies),
            TotalFEEDStudiesOverride = ConvertOverride<TotalFEEDStudiesOverrideDto, TotalFEEDStudiesOverride>(case_.TotalFEEDStudiesOverride),
        };

        return caseDto;
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
