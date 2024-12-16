using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos;
using api.Features.Projects.Update;
using api.Features.Stea.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectWithAssetsDto>();
        CreateMap<UpdateProjectDto, Project>();

        CreateMap<UpdateExplorationOperationalWellCostsDto, ExplorationOperationalWellCosts>();
        CreateMap<ExplorationOperationalWellCosts, ExplorationOperationalWellCostsDto>();
        CreateMap<UpdateDevelopmentOperationalWellCostsDto, DevelopmentOperationalWellCosts>();
        CreateMap<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>();
    }
}
