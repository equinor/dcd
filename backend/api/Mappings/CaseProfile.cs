using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<Case, CaseWithProfilesDto>();
        CreateMap<CessationWellsCost, CessationWellsCostDto>();
        CreateMap<CessationWellsCostOverride, CessationWellsCostOverrideDto>();
        CreateMap<CessationOffshoreFacilitiesCost, CessationOffshoreFacilitiesCostDto>();
        CreateMap<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto>();
        CreateMap<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto>();
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
        CreateMap<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto>();
        CreateMap<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto>();
        CreateMap<Image, ImageDto>();

        CreateMap<APIUpdateCaseWithProfilesDto, Case>();
        CreateMap<PROSPUpdateCaseDto, Case>();
        CreateMap<UpdateCessationWellsCostOverrideDto, CessationWellsCostOverride>();
        CreateMap<UpdateCessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>();
        CreateMap<UpdateCessationOnshoreFacilitiesCostProfileDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<UpdateTotalFeasibilityAndConceptStudiesOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<UpdateTotalFEEDStudiesOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<UpdateTotalOtherStudies, TotalOtherStudies>();
        CreateMap<UpdateWellInterventionCostProfileOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<UpdateHistoricCostCostProfileDto, HistoricCostCostProfile>();
        CreateMap<UpdateOnshoreRelatedOPEXCostProfileDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<UpdateAdditionalOPEXCostProfileDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateCaseDto, Case>();

        CreateMap<OpexCostProfile, OpexCostProfileDto>();
        CreateMap<CessationCost, CessationCostDto>();
        CreateMap<StudyCostProfile, StudyCostProfileDto>();
    }
}
