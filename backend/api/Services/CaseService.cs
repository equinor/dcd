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
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly ITopsideService _topsideService;
    private readonly ISurfService _surfService;
    private readonly ISubstructureService _substructureService;
    private readonly ITransportService _transportService;
    private readonly IExplorationService _explorationService;
    private readonly IWellProjectService _wellProjectService;
    private readonly ILogger<CaseService> _logger;

    public CaseService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, IDrainageStrategyService drainageStrategyService,
        ITopsideService topsideService, ISurfService surfService, ISubstructureService substructureService, ITransportService transportService,
        IExplorationService explorationService, IWellProjectService wellProjectService)
    {
        _context = context;
        _projectService = projectService;
        _drainageStrategyService = drainageStrategyService;
        _topsideService = topsideService;
        _surfService = surfService;
        _substructureService = substructureService;
        _transportService = transportService;
        _explorationService = explorationService;
        _wellProjectService = wellProjectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public async Task<ProjectDto> CreateCase(CaseDto caseDto)
    {
        var case_ = CaseAdapter.Convert(caseDto);
        if (case_.DG4Date == DateTimeOffset.MinValue)
        {
            case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }
        var project = await _projectService.GetProject(case_.ProjectId);
        case_.Project = project;
        _context.Cases!.Add(case_);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(project.Id);
    }

    public async Task<ProjectDto> NewCreateCase(CaseDto caseDto)
    {
        var caseItem = CaseAdapter.Convert(caseDto);
        var project = await _projectService.GetProject(caseItem.ProjectId);
        caseItem.Project = project;
        caseItem.CapexFactorFeasibilityStudies = 0.015;
        caseItem.CapexFactorFEEDStudies = 0.015;

        var createdCase = _context.Cases!.Add(caseItem);
        await _context.SaveChangesAsync();

        var drainageStrategyDto = new DrainageStrategyDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Drainage strategy",
            Description = ""
        };
        var drainageStrategy = await _drainageStrategyService.NewCreateDrainageStrategy(drainageStrategyDto, createdCase.Entity.Id);
        caseItem.DrainageStrategyLink = drainageStrategy.Id;

        var topsideDto = new TopsideDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Topside",
            Source = Source.ConceptApp,
        };
        var topside = await _topsideService.NewCreateTopside(topsideDto, createdCase.Entity.Id);
        caseItem.TopsideLink = topside.Id;

        var surfDto = new SurfDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Surf",
            Source = Source.ConceptApp,
        };
        var surf = await _surfService.NewCreateSurf(surfDto, createdCase.Entity.Id);
        caseItem.SurfLink = surf.Id;

        var substructureDto = new SubstructureDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Substructure",
            Source = Source.ConceptApp,
        };
        var substructure = await _substructureService.NewCreateSubstructure(substructureDto, createdCase.Entity.Id);
        caseItem.SubstructureLink = substructure.Id;

        var transportDto = new TransportDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Transport",
            Source = Source.ConceptApp,
        };
        var transport = await _transportService.NewCreateTransport(transportDto, createdCase.Entity.Id);
        caseItem.TransportLink = transport.Id;

        var explorationDto = new ExplorationDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Exploration",
        };
        var exploration = await _explorationService.NewCreateExploration(explorationDto, createdCase.Entity.Id);
        caseItem.ExplorationLink = exploration.Id;

        var wellProjectDto = new WellProjectDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "WellProject",
        };
        var wellProject = await _wellProjectService.NewCreateWellProject(wellProjectDto, createdCase.Entity.Id);
        caseItem.WellProjectLink = wellProject.Id;

        return await _projectService.GetProjectDto(project.Id);
    }

    public async Task<ProjectDto> UpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = await GetCase(updatedCaseDto.Id);
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<CaseDto> NewUpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = await GetCase(updatedCaseDto.Id);
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        await _context.SaveChangesAsync();
        return CaseDtoAdapter.Convert(await GetCase(caseItem.Id));
    }

    public async Task<ProjectDto> DeleteCase(Guid caseId)
    {
        var caseItem = await GetCase(caseId);
        _context.Cases!.Remove(caseItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<Case> GetCase(Guid caseId)
    {
        var caseItem = await _context.Cases!
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .FirstOrDefaultAsync(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }

    public async Task<IEnumerable<Case>> GetAll()
    {
        if (_context.Cases != null)
        {
            return await _context.Cases
                    .Include(c => c.TotalFeasibilityAndConceptStudies)
                    .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
                    .Include(c => c.TotalFEEDStudies)
                    .Include(c => c.TotalFEEDStudiesOverride)
                    .Include(c => c.WellInterventionCostProfile)
                    .Include(c => c.WellInterventionCostProfileOverride)
                    .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
                    .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
                    .Include(c => c.CessationWellsCost)
                    .Include(c => c.CessationWellsCostOverride)
                    .Include(c => c.CessationOffshoreFacilitiesCost)
                    .Include(c => c.CessationOffshoreFacilitiesCostOverride)
                    .ToListAsync();
        }
        else
        {
            _logger.LogInformation("No cases exists");
            return new List<Case>();
        }
    }
}
