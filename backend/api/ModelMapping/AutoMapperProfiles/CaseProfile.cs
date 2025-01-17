using api.Features.Cases.GetWithAssets.Dtos;
using api.Features.Profiles.Cases.AdditionalOpexCostProfiles.Dtos;
using api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides.Dtos;
using api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles.Dtos;
using api.Features.Profiles.Cases.CessationWellsCostOverrides.Dtos;
using api.Features.Profiles.Cases.HistoricCostCostProfiles.Dtos;
using api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles.Dtos;
using api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides.Dtos;
using api.Features.Profiles.Cases.TotalFeedStudiesOverrides.Dtos;
using api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles.Dtos;
using api.Features.Profiles.Cases.WellInterventionCostProfileOverrides.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
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
