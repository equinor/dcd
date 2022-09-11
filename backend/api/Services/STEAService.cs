using api.Adapters;
using api.Context;
using api.Dtos;

namespace api.Services;

public class STEAService
{
    private readonly ILogger<STEAService> _logger;

    private readonly ProjectService _projectService;

    public STEAService(DcdDbContext context, ILoggerFactory loggerFactory)
    {
        _projectService = new ProjectService(context, loggerFactory);
        _logger = loggerFactory.CreateLogger<STEAService>();
    }

    public STEAProjectDto GetInputToSTEA(Guid ProjectId)
    {
        var project = _projectService.GetProject(ProjectId);
        var sTEACaseDtos = new List<STEACaseDto>();
        foreach (var c in project.Cases!)
        {
            var projectDto = ProjectDtoAdapter.Convert(project);
            var caseDto = CaseDtoAdapter.Convert(c, projectDto);
            var sTEACaseDto = STEACaseDtoBuilder.Build(caseDto, projectDto);
            sTEACaseDtos.Add(sTEACaseDto);
        }

        return STEAProjectDtoBuilder.Build(ProjectDtoAdapter.Convert(project), sTEACaseDtos);
    }
}
