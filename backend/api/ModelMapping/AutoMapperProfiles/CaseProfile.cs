using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<TotalOtherStudiesCostProfile, TimeSeriesCostDto>();
        CreateMap<AdditionalOPEXCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalIncomeCostProfile, TimeSeriesCostDto>();
        CreateMap<CalculatedTotalCostCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostOverrideDto, TotalOtherStudiesCostProfile>();
        CreateMap<UpdateTimeSeriesCostDto, AdditionalOPEXCostProfile>();

        CreateMap<CreateTimeSeriesCostDto, TotalOtherStudiesCostProfile>();
        CreateMap<CreateTimeSeriesCostDto, AdditionalOPEXCostProfile>();
    }
}
