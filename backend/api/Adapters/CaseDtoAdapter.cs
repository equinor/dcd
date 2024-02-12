using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class CaseDtoAdapter
{
    public static CaseDto Convert(Case caseItem)
    {
        var caseDto = new CaseDto
        {
            Id = caseItem.Id,
            ProjectId = caseItem.ProjectId,
            Name = caseItem.Name,
            Description = caseItem.Description,
            ReferenceCase = caseItem.ReferenceCase,

            DGADate = caseItem.DGADate,
            DGBDate = caseItem.DGBDate,
            DGCDate = caseItem.DGCDate,
            APXDate = caseItem.APXDate,
            APZDate = caseItem.APZDate,
            DG0Date = caseItem.DG0Date,
            DG1Date = caseItem.DG1Date,
            DG2Date = caseItem.DG2Date,
            DG3Date = caseItem.DG3Date,
            DG4Date = caseItem.DG4Date,
            CreateTime = caseItem.CreateTime,
            ModifyTime = caseItem.ModifyTime,

            DrainageStrategyLink = caseItem.DrainageStrategyLink,
            WellProjectLink = caseItem.WellProjectLink,
            SurfLink = caseItem.SurfLink,
            SubstructureLink = caseItem.SubstructureLink,
            TopsideLink = caseItem.TopsideLink,
            TransportLink = caseItem.TransportLink,
            ExplorationLink = caseItem.ExplorationLink,

            ArtificialLift = caseItem.ArtificialLift,
            ProductionStrategyOverview = caseItem.ProductionStrategyOverview,
            ProducerCount = caseItem.ProducerCount,
            GasInjectorCount = caseItem.GasInjectorCount,
            WaterInjectorCount = caseItem.WaterInjectorCount,
            FacilitiesAvailability = caseItem.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = caseItem.CapexFactorFeasibilityStudies,
            CapexFactorFEEDStudies = caseItem.CapexFactorFEEDStudies,
            NPV = caseItem.NPV,
            BreakEven = caseItem.BreakEven,
            Host = caseItem.Host,

            SharepointFileId = caseItem.SharepointFileId,
            SharepointFileName = caseItem.SharepointFileName,
            SharepointFileUrl = caseItem.SharepointFileUrl,

            CessationWellsCost = Convert<CessationWellsCostDto, CessationWellsCost>(caseItem.CessationWellsCost) ?? new CessationWellsCostDto(),
            CessationWellsCostOverride = ConvertOverride<CessationWellsCostOverrideDto, CessationWellsCostOverride>(caseItem.CessationWellsCostOverride) ?? new CessationWellsCostOverrideDto(),

            CessationOffshoreFacilitiesCost = Convert<CessationOffshoreFacilitiesCostDto, CessationOffshoreFacilitiesCost>(caseItem.CessationOffshoreFacilitiesCost) ?? new CessationOffshoreFacilitiesCostDto(),
            CessationOffshoreFacilitiesCostOverride = ConvertOverride<CessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>(caseItem.CessationOffshoreFacilitiesCostOverride) ?? new CessationOffshoreFacilitiesCostOverrideDto(),

            TotalFeasibilityAndConceptStudies = Convert<TotalFeasibilityAndConceptStudiesDto, TotalFeasibilityAndConceptStudies>(caseItem.TotalFeasibilityAndConceptStudies) ?? new TotalFeasibilityAndConceptStudiesDto(),
            TotalFeasibilityAndConceptStudiesOverride = ConvertOverride<TotalFeasibilityAndConceptStudiesOverrideDto,
                TotalFeasibilityAndConceptStudiesOverride>(caseItem.TotalFeasibilityAndConceptStudiesOverride) ?? new TotalFeasibilityAndConceptStudiesOverrideDto(),

            TotalFEEDStudies = Convert<TotalFEEDStudiesDto, TotalFEEDStudies>(caseItem.TotalFEEDStudies) ?? new TotalFEEDStudiesDto(),
            TotalFEEDStudiesOverride = ConvertOverride<TotalFEEDStudiesOverrideDto, TotalFEEDStudiesOverride>(caseItem.TotalFEEDStudiesOverride) ?? new TotalFEEDStudiesOverrideDto(),

            WellInterventionCostProfile = Convert<WellInterventionCostProfileDto, WellInterventionCostProfile>(caseItem.WellInterventionCostProfile) ?? new WellInterventionCostProfileDto(),
            WellInterventionCostProfileOverride = ConvertOverride<WellInterventionCostProfileOverrideDto,
                WellInterventionCostProfileOverride>(caseItem.WellInterventionCostProfileOverride) ?? new WellInterventionCostProfileOverrideDto(),

            OffshoreFacilitiesOperationsCostProfile = Convert<OffshoreFacilitiesOperationsCostProfileDto, OffshoreFacilitiesOperationsCostProfile>(caseItem.OffshoreFacilitiesOperationsCostProfile) ?? new OffshoreFacilitiesOperationsCostProfileDto(),
            OffshoreFacilitiesOperationsCostProfileOverride = ConvertOverride<OffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>(caseItem.OffshoreFacilitiesOperationsCostProfileOverride) ?? new OffshoreFacilitiesOperationsCostProfileOverrideDto(),
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
