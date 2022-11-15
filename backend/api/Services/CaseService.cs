using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class CaseService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ILogger<CaseService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _context = context;
        _projectService = projectService;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto CreateCase(CaseDto caseDto)
    {
        var case_ = CaseAdapter.Convert(caseDto);
        if (case_.DG4Date == DateTimeOffset.MinValue)
        {
            case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }
        var project = _projectService.GetProjectWithBaggage(case_.ProjectId);
        case_.Project = project;
        _context.Cases!.Add(case_);
        _context.SaveChanges();
        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto NewCreateCase(CaseDto caseDto)
    {
        var drainageStrategyService = _serviceProvider.GetRequiredService<DrainageStrategyService>();
        var topsideService = _serviceProvider.GetRequiredService<TopsideService>();
        var surfService = _serviceProvider.GetRequiredService<SurfService>();
        var substructureService = _serviceProvider.GetRequiredService<SubstructureService>();
        var transportService = _serviceProvider.GetRequiredService<TransportService>();
        var explorationService = _serviceProvider.GetRequiredService<ExplorationService>();
        var wellProjectService = _serviceProvider.GetRequiredService<WellProjectService>();

        var case_ = CaseAdapter.Convert(caseDto);
        var project = _projectService.GetProjectWithBaggage(case_.ProjectId);
        case_.Project = project;
        case_.CapexFactorFeasibilityStudies = 0.015;
        case_.CapexFactorFEEDStudies = 0.015;

        var createdCase = _context.Cases!.Add(case_);
        _context.SaveChanges();

        var drainageStrategyDto = new DrainageStrategyDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Drainage strategy",
            Description = ""
        };
        var drainageStrategy = drainageStrategyService.NewCreateDrainageStrategy(drainageStrategyDto, createdCase.Entity.Id);
        case_.DrainageStrategyLink = drainageStrategy.Id;

        var topsideDto = new TopsideDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Topside",
        };
        var topside = topsideService.NewCreateTopside(topsideDto, createdCase.Entity.Id);
        case_.TopsideLink = topside.Id;

        var surfDto = new SurfDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Surf",
        };
        var surf = surfService.NewCreateSurf(surfDto, createdCase.Entity.Id);
        case_.SurfLink = surf.Id;

        var substructureDto = new SubstructureDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Substructure",
        };
        var substructure = substructureService.NewCreateSubstructure(substructureDto, createdCase.Entity.Id);
        case_.SubstructureLink = substructure.Id;

        var transportDto = new TransportDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Transport",
        };
        var transport = transportService.NewCreateTransport(transportDto, createdCase.Entity.Id);
        case_.TransportLink = transport.Id;

        var explorationDto = new ExplorationDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Exploration",
        };
        var exploration = explorationService.NewCreateExploration(explorationDto, createdCase.Entity.Id);
        case_.ExplorationLink = exploration.Id;

        var wellProjectDto = new WellProjectDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "WellProject",
        };
        var wellProject = wellProjectService.NewCreateWellProject(wellProjectDto, createdCase.Entity.Id);
        case_.WellProjectLink = wellProject.Id;

        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto DuplicateCase(Guid caseId)
    {
        var drainageStrategyService = _serviceProvider.GetRequiredService<DrainageStrategyService>();

        var topsideService = _serviceProvider.GetRequiredService<TopsideService>();
        var surfService = _serviceProvider.GetRequiredService<SurfService>();
        var substructureService = _serviceProvider.GetRequiredService<SubstructureService>();
        var transportService = _serviceProvider.GetRequiredService<TransportService>();

        var explorationService = _serviceProvider.GetRequiredService<ExplorationService>();
        var wellProjectService = _serviceProvider.GetRequiredService<WellProjectService>();

        var wellProjectWellService = _serviceProvider.GetRequiredService<WellProjectWellService>();
        var explorationWellService = _serviceProvider.GetRequiredService<ExplorationWellService>();

        var caseItem = GetCase(caseId);
        var sourceWellProjectId = caseItem.WellProjectLink;
        var sourceExplorationId = caseItem.ExplorationLink;
        caseItem.Id = new Guid();
        if (caseItem.DG4Date == DateTimeOffset.MinValue)
        {
            caseItem.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }
        var project = _projectService.GetProjectWithBaggage(caseItem.ProjectId);
        caseItem.Project = project;

        caseItem.Name += " - copy";
        _context.Cases!.Add(caseItem);

        drainageStrategyService.CopyDrainageStrategy(caseItem.DrainageStrategyLink, caseItem.Id);
        topsideService.CopyTopside(caseItem.TopsideLink, caseItem.Id);
        surfService.CopySurf(caseItem.SurfLink, caseItem.Id);
        substructureService.CopySubstructure(caseItem.SubstructureLink, caseItem.Id);
        transportService.CopyTransport(caseItem.TransportLink, caseItem.Id);
        var newWellProject = wellProjectService.CopyWellProject(caseItem.WellProjectLink, caseItem.Id);
        var newExploration = explorationService.CopyExploration(caseItem.ExplorationLink, caseItem.Id);

        wellProjectWellService.CopyWellProjectWell(sourceWellProjectId, newWellProject.Id);
        explorationWellService.CopyExplorationWell(sourceExplorationId, newExploration.Id);

        _context.SaveChanges();
        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto UpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = GetCase(updatedCaseDto.Id);
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        _context.SaveChanges();
        return _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public CaseDto NewUpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = GetCase(updatedCaseDto.Id);
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        _context.SaveChanges();
        return CaseDtoAdapter.Convert(GetCase(caseItem.Id));
    }

    public ProjectDto DeleteCase(Guid caseId)
    {
        var caseItem = GetCase(caseId);
        _context.Cases!.Remove(caseItem);
        _context.SaveChanges();
        return _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public Case GetCase(Guid caseId)
    {
        var caseItem = _context.Cases!
            .FirstOrDefault(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }

    public IEnumerable<Case> GetAll()
    {
        if (_context.Cases != null)
        {
            return _context.Cases;
        }
        else
        {
            _logger.LogInformation("No cases exists");
            return new List<Case>();
        }
    }
}
