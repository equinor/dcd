using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Create;
using api.Features.CaseProfiles.Dtos.Update;
using api.Features.Cases.Update;
using api.Features.Images.Dto;
using api.Features.Prosp.Models;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

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
        CreateMap<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto>();
        CreateMap<WellInterventionCostProfile, WellInterventionCostProfileDto>();
        CreateMap<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfile, OffshoreFacilitiesOperationsCostProfileDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto>();
        CreateMap<HistoricCostCostProfile, HistoricCostCostProfileDto>();
        CreateMap<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto>();
        CreateMap<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto>();
        CreateMap<CalculatedTotalIncomeCostProfile, CalculatedTotalIncomeCostProfileDto>();
        CreateMap<CalculatedTotalCostCostProfile, CalculatedTotalCostCostProfileDto>();

        CreateMap<Image, ImageDto>();

        CreateMap<UpdateCaseDto, Case>();
        CreateMap<ProspUpdateCaseDto, Case>();
        CreateMap<UpdateCessationWellsCostOverrideDto, CessationWellsCostOverride>();
        CreateMap<UpdateCessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>();
        CreateMap<UpdateCessationOnshoreFacilitiesCostProfileDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<UpdateTotalFeasibilityAndConceptStudiesOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<UpdateTotalFEEDStudiesOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<UpdateTotalOtherStudiesCostProfileDto, TotalOtherStudiesCostProfile>();
        CreateMap<UpdateWellInterventionCostProfileOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<UpdateHistoricCostCostProfileDto, HistoricCostCostProfile>();
        CreateMap<UpdateOnshoreRelatedOPEXCostProfileDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<UpdateAdditionalOPEXCostProfileDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateCessationWellsCostOverrideDto, CessationWellsCostOverride>();
        CreateMap<CreateCessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>();
        CreateMap<CreateCessationOnshoreFacilitiesCostProfileDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<CreateTotalFeasibilityAndConceptStudiesOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<CreateTotalFEEDStudiesOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<CreateTotalOtherStudiesCostProfileDto, TotalOtherStudiesCostProfile>();
        CreateMap<CreateWellInterventionCostProfileOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<CreateOffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<CreateHistoricCostCostProfileDto, HistoricCostCostProfile>();
        CreateMap<CreateOnshoreRelatedOPEXCostProfileDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<CreateAdditionalOPEXCostProfileDto, AdditionalOPEXCostProfile>();

        CreateMap<OpexCostProfile, OpexCostProfileDto>();
        CreateMap<CessationCost, CessationCostDto>();
        CreateMap<StudyCostProfile, StudyCostProfileDto>();
    }
}
