using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Update;
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
