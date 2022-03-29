
using api.Dtos;

using Microsoft.IdentityModel.Tokens;

namespace api.Adapters
{
    public static class STEAProjectDtoBuilder
    {
        public static STEAProjectDto Build(ProjectDto project, List<STEACaseDto> sTEACaseDtos)
        {
            var sTEAprojectDto = new STEAProjectDto();
            sTEAprojectDto.Name = project.Name;
            sTEAprojectDto.STEACases = new List<STEACaseDto>();
            int[] startYears = new int[sTEACaseDtos.Count()];
            int counter = 0;
            foreach (STEACaseDto c in sTEACaseDtos)
            {
                startYears[counter++] = c.StartYear;
                sTEAprojectDto.STEACases.Add(c);
            }

            if (!startYears.IsNullOrEmpty())
            {
                Array.Sort(startYears);
                sTEAprojectDto.StartYear = Array.Find(startYears, e => e > 0);
            }
            return sTEAprojectDto;
        }
    }
}
