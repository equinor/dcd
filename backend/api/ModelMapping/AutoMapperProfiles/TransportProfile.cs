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
        CreateMap<TransportCessationCostProfile, TimeSeriesCostDto>();
    }
}
