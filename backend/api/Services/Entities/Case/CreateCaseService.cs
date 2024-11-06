using api.Dtos;
using api.Models;
using api.Repositories;

namespace api.Services;

public class CreateCaseService(
    ICaseRepository caseRepository,
    IProjectService projectService,
    IMapperService mapperService)
    : ICreateCaseService
{
    public async Task<ProjectWithAssetsDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto)
    {
        var caseItem = new Case();
        mapperService.MapToEntity(createCaseDto, caseItem, Guid.Empty);

        var project = await projectService.GetProject(projectId);
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

        await caseRepository.AddCase(caseItem);

        return await projectService.GetProjectDto(project.Id);
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
            CostProfileOverride = new TopsideCostProfileOverride
            {
                Override = true,
            },
        };
    }

    private static Surf CreateSurf(Project project)
    {
        return new Surf
        {
            Name = "Surf",
            Project = project,
            CostProfileOverride = new SurfCostProfileOverride
            {
                Override = true,
            },
        };
    }

    private static Substructure CreateSubstructure(Project project)
    {
        return new Substructure
        {
            Name = "Substructure",
            Project = project,
            CostProfileOverride = new SubstructureCostProfileOverride
            {
                Override = true,
            },
        };
    }

    private static Transport CreateTransport(Project project)
    {
        return new Transport
        {
            Name = "Transport",
            Project = project,
            CostProfileOverride = new TransportCostProfileOverride
            {
                Override = true,
            },
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
