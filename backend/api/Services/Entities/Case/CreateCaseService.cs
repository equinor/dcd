using api.Context;
using api.Dtos;
using api.Models;
using api.Repositories;

namespace api.Services;

public class CreateCaseService : ICreateCaseService
{
    private readonly ICaseRepository _caseRepository;
    private readonly IProjectService _projectService;
    private readonly IMapperService _mapperService;

    public CreateCaseService(
        DcdDbContext context,
        ICaseRepository caseRepository,
        IProjectService projectService,
        IMapperService mapperService
    )
    {
        _caseRepository = caseRepository;
        _projectService = projectService;
        _mapperService = mapperService;
    }

    public async Task<ProjectWithAssetsDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto)
    {
        var caseItem = new Case();
        _mapperService.MapToEntity(createCaseDto, caseItem, Guid.Empty);

        var project = await _projectService.GetProject(projectId);
        caseItem.Project = project;
        caseItem.CapexFactorFeasibilityStudies = 0.015;
        caseItem.CapexFactorFEEDStudies = 0.015;

        if (caseItem.DG4Date == DateTimeOffset.MinValue)
        {
            caseItem.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
        }

        caseItem.CreateTime = DateTimeOffset.UtcNow;

        caseItem.DrainageStrategy = CreateDrainageStrategy(project);
        caseItem.Topside = CreateTopside(project);
        caseItem.Surf = CreateSurf(project);
        caseItem.Substructure = CreateSubstructure(project);
        caseItem.Transport = CreateTransport(project);
        caseItem.Exploration = CreateExploration(project);
        caseItem.WellProject = CreateWellProject(project);

        await _caseRepository.AddCase(caseItem);

        return await _projectService.GetProjectDto(project.Id);
    }

    private static DrainageStrategy CreateDrainageStrategy(Project project)
    {
        return new DrainageStrategy
        {
            Name = "Drainage Strategy",
            Description = "Drainage Strategy",
            Project = project,
        };
    }

    private static Topside CreateTopside(Project project)
    {
        return new Topside
        {
            Name = "Topside",
            Project = project,
        };
    }

    private static Surf CreateSurf(Project project)
    {
        return new Surf
        {
            Name = "Surf",
            Project = project,
        };
    }

    private static Substructure CreateSubstructure(Project project)
    {
        return new Substructure
        {
            Name = "Substructure",
            Project = project,
        };
    }

    private static Transport CreateTransport(Project project)
    {
        return new Transport
        {
            Name = "Transport",
            Project = project,
        };
    }

    private static Exploration CreateExploration(Project project)
    {
        return new Exploration
        {
            Name = "Exploration",
            Project = project,
        };
    }

    private static WellProject CreateWellProject(Project project)
    {
        return new WellProject
        {
            Name = "Well Project",
            Project = project,
        };
    }
}
