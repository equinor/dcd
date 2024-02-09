using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class TransportProfile : Profile
{
    public TransportProfile()
    {
        CreateMap<Transport, TransportDto>();
        CreateMap<TransportCostProfile, TransportCostProfileDto>();
        CreateMap<TransportCostProfileOverride, TransportCostProfileOverrideDto>();
        CreateMap<TransportCessationCostProfile, TransportCessationCostProfileDto>();

        CreateMap<UpdateTransportDto, Transport>();
        CreateMap<UpdateTransportCostProfileOverrideDto, TransportCostProfileOverride>();

        CreateMap<CreateTransportDto, Transport>();
    }
}