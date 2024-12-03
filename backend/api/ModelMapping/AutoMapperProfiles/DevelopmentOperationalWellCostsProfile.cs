using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class DevelopmentOperationalWellCostsProfile : Profile
{
    public DevelopmentOperationalWellCostsProfile()
    {
        CreateMap<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>().ReverseMap();
        CreateMap<UpdateDevelopmentOperationalWellCostsDto, DevelopmentOperationalWellCosts>();
    }
}
