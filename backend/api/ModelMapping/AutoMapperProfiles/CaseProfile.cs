using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<CessationOnshoreFacilitiesCostProfile, TimeSeriesCostDto>();
        CreateMap<TotalFEEDStudies, TimeSeriesCostDto>();
        CreateMap<TotalFEEDStudiesOverride, TimeSeriesCostOverrideDto>();
        CreateMap<TotalOtherStudiesCostProfile, TimeSeriesCostDto>();
        CreateMap<HistoricCostCostProfile, TimeSeriesCostDto>();
        CreateMap<OnshoreRelatedOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<AdditionalOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalIncomeCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalCostCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalOtherStudiesCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, HistoricCostCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateTimeSeriesCostDto, CessationOnshoreFacilitiesCostProfile>();
        CreateMap<CreateTimeSeriesCostOverrideDto, TotalFEEDStudiesOverride>();
        CreateMap<CreateTimeSeriesCostDto, TotalOtherStudiesCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, HistoricCostCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, AdditionalOPEXCostProfile>();
    }
}
