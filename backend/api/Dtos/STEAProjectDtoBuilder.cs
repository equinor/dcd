using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public static class STEAProjectDtoBuilder
    {
        public static STEAProjectDto Build(ProjectDto project)
        {
            var sTEAprojectDto = new STEAProjectDto();
            sTEAprojectDto.Name = project.Name;
            sTEAprojectDto.STEACases = new List<STEACaseDto>();
            foreach (CaseDto c in project.Cases)
            {
                sTEAprojectDto.STEACases.Add(STEACaseDtoBuilder.Build(c, project));
            }
            return sTEAprojectDto;
        }
    }
}
