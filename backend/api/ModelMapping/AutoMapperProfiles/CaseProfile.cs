using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<CessationOnshoreFacilitiesCostProfile, TimeSeriesCostDto>();
        CreateMap<TotalFeasibilityAndConceptStudies, TimeSeriesCostDto>();
        CreateMap<TotalFeasibilityAndConceptStudiesOverride, TimeSeriesCostOverrideDto>();
        CreateMap<TotalFEEDStudies, TimeSeriesCostDto>();
        CreateMap<TotalFEEDStudiesOverride, TimeSeriesCostOverrideDto>();
        CreateMap<TotalOtherStudiesCostProfile, TimeSeriesCostDto>();
        CreateMap<WellInterventionCostProfile, TimeSeriesCostDto>();
        CreateMap<WellInterventionCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfile, TimeSeriesCostDto>();
        CreateMap<OffshoreFacilitiesOperationsCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<HistoricCostCostProfile, TimeSeriesCostDto>();
        CreateMap<OnshoreRelatedOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<AdditionalOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalIncomeCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalCostCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalOtherStudiesCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<UpdateTimeSeriesCostDto, HistoricCostCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateTimeSeriesCostDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<CreateTimeSeriesCostOverrideDto, TotalFeasibilityAndConceptStudiesOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<CreateTimeSeriesCostDto, TotalOtherStudiesCostProfile>();
        CreateMap<CreateTimeSeriesCostOverrideDto, WellInterventionCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, OffshoreFacilitiesOperationsCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostDto, HistoricCostCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, AdditionalOPEXCostProfile>();
    }
}
