using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Api.Services.FusionIntegration;

namespace api.Services;

public class STEAService
{

    private readonly ProjectService _projectService;
    private readonly ILogger<STEAService> _logger;

    public STEAService(DcdDbContext context, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _projectService = new ProjectService(context, loggerFactory, serviceProvider);
        _logger = loggerFactory.CreateLogger<STEAService>();
    }

    public STEAProjectDto GetInputToSTEA(Guid ProjectId)
    {

        var project = _projectService.GetProjectWithAssets(ProjectId);
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
