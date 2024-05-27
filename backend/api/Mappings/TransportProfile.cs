using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class TransportProfile : Profile
{
    public TransportProfile()
    {
        CreateMap<Transport, TransportWithProfilesDto>();
        CreateMap<TransportCostProfile, TransportCostProfileDto>();
        CreateMap<TransportCostProfileOverride, TransportCostProfileOverrideDto>();
        CreateMap<TransportCessationCostProfile, TransportCessationCostProfileDto>();

        CreateMap<APIUpdateTransportWithProfilesDto, Transport>();
        CreateMap<PROSPUpdateTransportDto, Transport>();
        CreateMap<UpdateTransportCostProfileDto, TransportCostProfile>();
        CreateMap<UpdateTransportCostProfileOverrideDto, TransportCostProfileOverride>();

        CreateMap<CreateTransportDto, Transport>();
    }
}
