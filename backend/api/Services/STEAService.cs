using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Services;

public class STEAService : ISTEAService
{
    private readonly IProjectService _projectService;
    private readonly ILogger<STEAService> _logger;
    private readonly IMapper _mapper;

    public STEAService(
            ILoggerFactory loggerFactory,
            IProjectService projectService,
            IMapper mapper
        )
    {
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<STEAService>();
        _mapper = mapper;
    }

    public async Task<STEAProjectDto> GetInputToSTEA(Guid ProjectId)
    {
        var project = await _projectService.GetProject(ProjectId);
        var sTEACaseDtos = new List<STEACaseDto>();
        var projectDto = _mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
        foreach (Case c in project.Cases!)
        {
            var caseDto = _mapper.Map<CaseWithProfilesDto>(c);
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
