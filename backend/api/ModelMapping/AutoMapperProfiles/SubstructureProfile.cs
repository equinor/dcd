using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class SubstructureProfile : Profile
{
    public SubstructureProfile()
    {
        CreateMap<Substructure, SubstructureDto>();
        CreateMap<SubstructureCostProfile, TimeSeriesCostDto>();
        CreateMap<SubstructureCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<SubstructureCessationCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostDto, SubstructureCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, SubstructureCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, SubstructureCostProfileOverride>();
    }
}
