using api.Features.Assets.CaseAssets.Transports;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Transports.TransportCostProfileOverrides.Dtos;
using api.Features.Profiles.Transports.TransportCostProfiles.Dtos;
using api.Features.Stea.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class TransportProfile : Profile
{
    public TransportProfile()
    {
        CreateMap<Transport, TransportDto>();
        CreateMap<Transport, TransportWithProfilesDto>();
        CreateMap<TransportCostProfile, TransportCostProfileDto>();
        CreateMap<TransportCostProfileOverride, TransportCostProfileOverrideDto>();
        CreateMap<TransportCessationCostProfile, TransportCessationCostProfileDto>();

        CreateMap<UpdateTransportDto, Transport>();
        CreateMap<ProspUpdateTransportDto, Transport>();
        CreateMap<UpdateTransportCostProfileDto, TransportCostProfile>();
        CreateMap<UpdateTransportCostProfileOverrideDto, TransportCostProfileOverride>();
        CreateMap<CreateTransportCostProfileOverrideDto, TransportCostProfileOverride>();
    }
}
