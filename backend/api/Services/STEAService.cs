using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Services;

public class STEAService(
    ILoggerFactory loggerFactory,
    IProjectService projectService,
    IMapper mapper) : ISTEAService
{
    private readonly ILogger<STEAService> _logger = loggerFactory.CreateLogger<STEAService>();

    public async Task<STEAProjectDto> GetInputToSTEA(Guid ProjectId)
    {
        var project = await projectService.GetProjectWithCasesAndAssets(ProjectId);
        var sTEACaseDtos = new List<STEACaseDto>();
        var projectDto = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
        foreach (Case c in project.Cases!)
        {
            if (c.Archived) { continue; }
            var caseDto = mapper.Map<CaseWithProfilesDto>(c);
            if (projectDto == null || caseDto == null)
            {
                _logger.LogError("Failed to map project or case to dto");
                throw new Exception("Failed to map project or case to dto");
            }
            STEACaseDto sTEACaseDto = STEACaseDtoBuilder.Build(caseDto, projectDto);
            sTEACaseDtos.Add(sTEACaseDto);
        }

        if (projectDto == null)
        {
            _logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }
        return STEAProjectDtoBuilder.Build(projectDto, sTEACaseDtos);
    }
}
