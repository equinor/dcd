using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
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

        CreateMap<APIUpdateTransportDto, Transport>();
        CreateMap<APIUpdateTransportWithProfilesDto, Transport>();
        CreateMap<PROSPUpdateTransportDto, Transport>();
        CreateMap<UpdateTransportCostProfileDto, TransportCostProfile>();
        CreateMap<UpdateTransportCostProfileOverrideDto, TransportCostProfileOverride>();
        CreateMap<CreateTransportCostProfileOverrideDto, TransportCostProfileOverride>();

        CreateMap<CreateTransportDto, Transport>();
    }
}
