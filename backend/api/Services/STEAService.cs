using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

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

            var project = _projectService.GetProject(ProjectId);
            List<STEACaseDto> sTEACaseDtos = new List<STEACaseDto>();
            foreach (Case c in project.Cases!)
            {
                ProjectDto projectDto = ProjectDtoAdapter.Convert(project);
                CaseDto caseDto = CaseDtoAdapter.Convert(c);
                STEACaseDto sTEACaseDto = STEACaseDtoBuilder.Build(caseDto, projectDto);
                sTEACaseDtos.Add(sTEACaseDto);
            }
            return STEAProjectDtoBuilder.Build(ProjectDtoAdapter.Convert(project), sTEACaseDtos);
        }

    }
}
