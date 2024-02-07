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
        CreateMap<WellInterventionCostProfile, WellInterventionCostProfileDto>();
        CreateMap<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfile, OffshoreFacilitiesOperationsCostProfileDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto>();
        // If you need to map in reverse direction
        CreateMap<UpdateCaseDto, Case>();
        CreateMap<CreateCaseDto, Case>();
    }
}