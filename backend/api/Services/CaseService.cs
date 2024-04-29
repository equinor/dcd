using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

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
    private readonly IMapper _mapper;

    public CaseService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, IDrainageStrategyService drainageStrategyService,
        ITopsideService topsideService, ISurfService surfService, ISubstructureService substructureService, ITransportService transportService,
        IExplorationService explorationService, IWellProjectService wellProjectService, IMapper mapper)
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
        _mapper = mapper;
    }

    public async Task<ProjectDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto)
    {
        var caseItem = _mapper.Map<Case>(createCaseDto);
        if (caseItem == null)
        {
            throw new ArgumentNullException(nameof(caseItem));
        }
        var project = await _projectService.GetProject(projectId);
        caseItem.Project = project;
        caseItem.CapexFactorFeasibilityStudies = 0.015;
        caseItem.CapexFactorFEEDStudies = 0.015;

        caseItem.CreateTime = DateTimeOffset.UtcNow;

        var createdCase = _context.Cases!.Add(caseItem);
        await _context.SaveChangesAsync();

        var drainageStrategyDto = new CreateDrainageStrategyDto
        {
            Name = "Drainage strategy",
            Description = ""
        };
        var drainageStrategy = await _drainageStrategyService.NewCreateDrainageStrategy(projectId, createdCase.Entity.Id, drainageStrategyDto);
        caseItem.DrainageStrategyLink = drainageStrategy.Id;

        var topsideDto = new CreateTopsideDto
        {
            Name = "Topside",
            Source = Source.ConceptApp,
        };
        var topside = await _topsideService.CreateTopside(projectId, createdCase.Entity.Id, topsideDto);
        caseItem.TopsideLink = topside.Id;

        var surfDto = new CreateSurfDto
        {
            Name = "Surf",
            Source = Source.ConceptApp,
        };
        var surf = await _surfService.CreateSurf(projectId, createdCase.Entity.Id, surfDto);
        caseItem.SurfLink = surf.Id;

        var substructureDto = new CreateSubstructureDto
        {
            Name = "Substructure",
            Source = Source.ConceptApp,
        };
        var substructure = await _substructureService.CreateSubstructure(projectId, createdCase.Entity.Id, substructureDto);
        caseItem.SubstructureLink = substructure.Id;

        var transportDto = new CreateTransportDto
        {
            Name = "Transport",
            Source = Source.ConceptApp,
        };
        var transport = await _transportService.CreateTransport(projectId, createdCase.Entity.Id, transportDto);
        caseItem.TransportLink = transport.Id;

        var explorationDto = new CreateExplorationDto
        {
            Name = "Exploration",
        };
        var exploration = await _explorationService.CreateExploration(projectId, createdCase.Entity.Id, explorationDto);
        caseItem.ExplorationLink = exploration.Id;

        var wellProjectDto = new CreateWellProjectDto
        {
            Name = "WellProject",
        };
        var wellProject = await _wellProjectService.CreateWellProject(projectId, createdCase.Entity.Id, wellProjectDto);
        caseItem.WellProjectLink = wellProject.Id;

        return await _projectService.GetProjectDto(project.Id);
    }

    public async Task<ProjectDto> UpdateCase<TDto>(Guid caseId, TDto updatedCaseDto)
        where TDto : BaseUpdateCaseDto
    {
        var caseItem = await GetCase(caseId);

        _mapper.Map(updatedCaseDto, caseItem);

        _context.Cases!.Update(caseItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(caseItem.ProjectId);
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
            .Include(c => c.TotalOtherStudies)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
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
                    .Include(c => c.TotalOtherStudies)
                    .Include(c => c.HistoricCostCostProfile)
                    .Include(c => c.WellInterventionCostProfile)
                    .Include(c => c.WellInterventionCostProfileOverride)
                    .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
                    .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
                    .Include(c => c.OnshoreRelatedOPEXCostProfile)
                    .Include(c => c.AdditionalOPEXCostProfile)
                    .Include(c => c.CessationWellsCost)
                    .Include(c => c.CessationWellsCostOverride)
                    .Include(c => c.CessationOffshoreFacilitiesCost)
                    .Include(c => c.CessationOffshoreFacilitiesCostOverride)
                    .Include(c => c.CessationOnshoreFacilitiesCostProfile)
                    .ToListAsync();
        }
        else
        {
            _logger.LogInformation("No cases exists");
            return new List<Case>();
        }
    }
}
