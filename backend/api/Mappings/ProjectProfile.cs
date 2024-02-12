using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>();

        CreateMap<UpdateProjectDto, Project>();
    }
}
