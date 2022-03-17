using api.Dtos;

using Microsoft.IdentityModel.Tokens;

namespace api.Adapters
{
    public static class STEAProjectDtoBuilder
    {
        public static STEAProjectDto Build(ProjectDto project)
        {
            var sTEAprojectDto = new STEAProjectDto();
            sTEAprojectDto.Name = project.Name;
            sTEAprojectDto.STEACases = new List<STEACaseDto>();
            List<int> startYears = new List<int>();

            foreach (CaseDto c in project.Cases!)
            {
                var sTEACaseDto = STEACaseDtoBuilder.Build(c, project);
                sTEAprojectDto.STEACases.Add(sTEACaseDto);
                startYears.Add(sTEACaseDto.StartYear);
            }

            if (!startYears.IsNullOrEmpty())
            {
                startYears.Sort();
                sTEAprojectDto.StartYear = startYears.Find(e => e > 0);
            }
            return sTEAprojectDto;
        }
    }
}
