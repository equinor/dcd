using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class TransportProfile : Profile
{
    public TransportProfile()
    {
        CreateMap<Transport, TransportDto>();
        CreateMap<TransportCostProfile, TimeSeriesCostDto>();
        CreateMap<TransportCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<TransportCessationCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostDto, TransportCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TransportCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, TransportCostProfileOverride>();
    }
}
