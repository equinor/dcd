using api.Adapters;
using api.Context;
using api.Dtos;

namespace api.Services
{
    public class STEAService
    {

        private readonly ProjectService _projectService;


        public STEAService(DcdDbContext context)
        {
            _projectService = new ProjectService(context);
        }

        public STEAProjectDto GetInputToSTEA(Guid ProjectId)
        {
            var project = ProjectDtoAdapter.Convert(_projectService.GetProject(ProjectId));
            return STEAProjectDtoBuilder.Build(project);
        }

    }
}
