using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectMembers.Get;
using api.Features.Projects.Update;
using api.Features.Revisions.Get;
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
        CreateMap<ProjectMember, ProjectMemberDto>();

        CreateMap<UpdateExplorationOperationalWellCostsDto, ExplorationOperationalWellCosts>();
        CreateMap<ExplorationOperationalWellCosts, ExplorationOperationalWellCostsDto>();
        CreateMap<UpdateDevelopmentOperationalWellCostsDto, DevelopmentOperationalWellCosts>();
        CreateMap<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>();
        CreateMap<RevisionDetails, RevisionDetailsDto>();
        CreateMap<Project, RevisionWithCasesDto>();
    }
}
