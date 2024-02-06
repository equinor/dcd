using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class STEAService : ISTEAService
{
    private readonly IProjectService _projectService;
    private readonly ILogger<STEAService> _logger;

    public STEAService(DcdDbContext context, ILoggerFactory loggerFactory, IProjectService projectService)
    {
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<STEAService>();
    }

    public async Task<STEAProjectDto> GetInputToSTEA(Guid ProjectId)
    {
        var project = await _projectService.GetProject(ProjectId);
        var sTEACaseDtos = new List<STEACaseDto>();
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
