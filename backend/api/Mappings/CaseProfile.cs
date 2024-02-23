using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<Case, CaseDto>();
        CreateMap<CessationWellsCost, CessationWellsCostDto>();
        CreateMap<CessationWellsCostOverride, CessationWellsCostOverrideDto>();
        CreateMap<CessationOffshoreFacilitiesCost, CessationOffshoreFacilitiesCostDto>();
        CreateMap<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto>();
        CreateMap<TotalFeasibilityAndConceptStudies, TotalFeasibilityAndConceptStudiesDto>();
        CreateMap<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto>();
        CreateMap<TotalFEEDStudies, TotalFEEDStudiesDto>();
        CreateMap<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto>();
        CreateMap<TotalOtherStudies, TotalOtherStudiesDto>();
        CreateMap<WellInterventionCostProfile, WellInterventionCostProfileDto>();
        CreateMap<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfile, OffshoreFacilitiesOperationsCostProfileDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto>();
        CreateMap<HistoricCostCostProfile, HistoricCostCostProfileDto>();
        CreateMap<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto>();

        CreateMap<UpdateCaseDto, Case>();
        CreateMap<UpdateCessationWellsCostOverrideDto, CessationWellsCostOverride>();
        CreateMap<UpdateCessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>();
        CreateMap<UpdateTotalFeasibilityAndConceptStudiesOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<UpdateTotalFEEDStudiesOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<UpdateWellInterventionCostProfileOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();

        CreateMap<CreateCaseDto, Case>();

        CreateMap<OpexCostProfile, OpexCostProfileDto>();
        CreateMap<CessationCost, CessationCostDto>();
        CreateMap<StudyCostProfile, StudyCostProfileDto>();
    }
}
