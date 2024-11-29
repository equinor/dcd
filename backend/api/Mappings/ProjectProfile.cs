using api.Dtos;
using api.Features.Projects.ProjectMember.Create;
using api.Features.Revision.Get;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectWithAssetsDto>();
        CreateMap<Project, ProjectWithCasesDto>();
        CreateMap<Project, ProjectDto>();
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
