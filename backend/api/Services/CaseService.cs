using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseService : ICaseService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IDrainageStrategyService drainageStrategyService;
    private readonly ITopsideService topsideService;
    private readonly ISurfService surfService;
    private readonly ISubstructureService substructureService;
    private readonly ITransportService transportService;
    private readonly IExplorationService explorationService;
    private readonly IWellProjectService wellProjectService;
    private readonly ILogger<CaseService> _logger;

    public CaseService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, IDrainageStrategyService drainageStrategyService,
        ITopsideService topsideService, ISurfService surfService, ISubstructureService substructureService, ITransportService transportService,
        IExplorationService explorationService, IWellProjectService wellProjectService)
    {
        _context = context;
        _projectService = projectService;
        this.drainageStrategyService = drainageStrategyService;
        this.topsideService = topsideService;
        this.surfService = surfService;
        this.substructureService = substructureService;
        this.transportService = transportService;
        this.explorationService = explorationService;
        this.wellProjectService = wellProjectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto CreateCase(CaseDto caseDto)
    {
        var case_ = CaseAdapter.Convert(caseDto);
        if (case_.DG4Date == DateTimeOffset.MinValue)
        {
            case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }
        var project = _projectService.GetProject(case_.ProjectId);
        case_.Project = project;
        _context.Cases!.Add(case_);
        _context.SaveChanges();
        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto NewCreateCase(CaseDto caseDto)
    {
        var case_ = CaseAdapter.Convert(caseDto);
        var project = _projectService.GetProject(case_.ProjectId);
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
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
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
            return _context.Cases
                    .Include(c => c.TotalFeasibilityAndConceptStudies)
                    .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
                    .Include(c => c.TotalFEEDStudies)
                    .Include(c => c.TotalFEEDStudiesOverride);
        }
        else
        {
            _logger.LogInformation("No cases exists");
            return new List<Case>();
        }
    }
}
