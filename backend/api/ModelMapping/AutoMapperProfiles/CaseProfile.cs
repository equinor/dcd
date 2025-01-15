using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles;
using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides;
using api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides.Dtos;
using api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides.Dtos;
using api.Features.CaseProfiles.Services.HistoricCostCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.CaseProfiles.Services.OnshoreRelatedOpexCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides.Dtos;
using api.Features.CaseProfiles.Services.TotalFeedStudiesOverrides.Dtos;
using api.Features.CaseProfiles.Services.TotalOtherStudiesCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides.Dtos;
using api.Features.Stea.Dtos;
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
        CreateMap<TotalFEEDStudiesOverride, TotalFeedStudiesOverrideDto>();
        CreateMap<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto>();
        CreateMap<WellInterventionCostProfile, WellInterventionCostProfileDto>();
        CreateMap<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfile, OffshoreFacilitiesOperationsCostProfileDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto>();
        CreateMap<HistoricCostCostProfile, HistoricCostCostProfileDto>();
        CreateMap<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOpexCostProfileDto>();
        CreateMap<AdditionalOPEXCostProfile, AdditionalOpexCostProfileDto>();
        CreateMap<CalculatedTotalIncomeCostProfile, CalculatedTotalIncomeCostProfileDto>();
        CreateMap<CalculatedTotalCostCostProfile, CalculatedTotalCostCostProfileDto>();

        CreateMap<UpdateCessationWellsCostOverrideDto, CessationWellsCostOverride>();
        CreateMap<UpdateCessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>();
        CreateMap<UpdateCessationOnshoreFacilitiesCostProfileDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<UpdateTotalFeasibilityAndConceptStudiesOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<UpdateTotalFeedStudiesOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<UpdateTotalOtherStudiesCostProfileDto, TotalOtherStudiesCostProfile>();
        CreateMap<UpdateWellInterventionCostProfileOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<UpdateHistoricCostCostProfileDto, HistoricCostCostProfile>();
        CreateMap<UpdateOnshoreRelatedOpexCostProfileDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<UpdateAdditionalOpexCostProfileDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateCessationWellsCostOverrideDto, CessationWellsCostOverride>();
        CreateMap<CreateCessationOffshoreFacilitiesCostOverrideDto, CessationOffshoreFacilitiesCostOverride>();
        CreateMap<CreateCessationOnshoreFacilitiesCostProfileDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<CreateTotalFeasibilityAndConceptStudiesOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<CreateTotalFeedStudiesOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<CreateTotalOtherStudiesCostProfileDto, TotalOtherStudiesCostProfile>();
        CreateMap<CreateWellInterventionCostProfileOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<CreateOffshoreFacilitiesOperationsCostProfileOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<CreateHistoricCostCostProfileDto, HistoricCostCostProfile>();
        CreateMap<CreateOnshoreRelatedOpexCostProfileDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<CreateAdditionalOpexCostProfileDto, AdditionalOPEXCostProfile>();
    }
}
