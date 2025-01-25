using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<TotalOtherStudiesCostProfile, TimeSeriesCostDto>();
        CreateMap<OnshoreRelatedOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<AdditionalOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalIncomeCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalCostCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalOtherStudiesCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateTimeSeriesCostDto, TotalOtherStudiesCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, OnshoreRelatedOPEXCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, AdditionalOPEXCostProfile>();
    }
}
