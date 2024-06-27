using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

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
