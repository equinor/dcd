using api.Dtos;
using api.Features.Projects.GetWithAssets;
using api.Helpers;

namespace api.Features.Stea.Dtos;

public static class SteaProjectDtoBuilder
{
    public static SteaProjectDto Build(ProjectWithAssetsDto project, List<SteaCaseDto> steaCaseDtos)
    {
        var steaCases = new List<SteaCaseDto>();

        var startYear = 0;
        var startYears = new int[steaCaseDtos.Count];
        var counter = 0;

        foreach (var c in steaCaseDtos)
        {
            startYears[counter++] = c.StartYear;
            steaCases.Add(c);
        }

        if (!startYears.IsNullOrEmpty())
        {
            Array.Sort(startYears);
            startYear = Array.Find(startYears, e => e > 0);
        }

        return new SteaProjectDto
        {
            Name = project.Name,
            SteaCases = steaCases,
            StartYear = startYear
        };
    }
}
