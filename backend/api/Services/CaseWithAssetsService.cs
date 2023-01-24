using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class CaseWithAssetsService : ICaseWithAssetsService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IExplorationService _explorationService;
    private readonly ISurfService _surfService;
    private readonly ISubstructureService _substructureService;
    private readonly ITransportService _transportService;
    private readonly ITopsideService _topsideService;
    private readonly IExplorationWellService _explorationWellService;
    private readonly IWellProjectWellService _wellProjectWellService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<CaseService> _logger;

    public CaseWithAssetsService(DcdDbContext context, IProjectService projectService, ICaseService caseService,
    IDrainageStrategyService drainageStrategyService, IWellProjectService wellProjectService, IExplorationService explorationService,
    ISurfService surfService, ISubstructureService substructureService, ITransportService transportService, ITopsideService topsideService,
    IExplorationWellService explorationWellService, IWellProjectWellService wellProjectWellService,
    ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    ILoggerFactory loggerFactory)
    {
        _context = context;

        _projectService = projectService;
        _caseService = caseService;

        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _explorationService = explorationService;
        _surfService = surfService;
        _substructureService = substructureService;
        _transportService = transportService;
        _topsideService = topsideService;

        _explorationWellService = explorationWellService;
        _wellProjectWellService = wellProjectWellService;

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto UpdateCaseWithAssets(CaseWithAssetsWrapperDto wrapper)
    {
        var project = _projectService.GetProjectWithoutAssets(wrapper.CaseDto.ProjectId);

        var updatedCaseDto = UpdateCase(wrapper.CaseDto);

        UpdateDrainageStrategy(wrapper.DrainageStrategyDto, project.PhysicalUnit);
        UpdateWellProject(wrapper.WellProjectDto);
        UpdateExploration(wrapper.ExplorationDto);
        UpdateSurf(wrapper.SurfDto);
        UpdateSubstructure(wrapper.SubstructureDto);
        UpdateTransport(wrapper.TransportDto);
        UpdateTopside(wrapper.TopsideDto);

        if (wrapper.ExplorationWellDto?.Length > 0)
        {
            CreateAndUpdateExplorationWells(wrapper.ExplorationWellDto, updatedCaseDto.Id);
        }

        if (wrapper.WellProjectWellDtos?.Length > 0)
        {
            CreateAndUpdateWellProjectWells(wrapper.WellProjectWellDtos, updatedCaseDto.Id);
        }

        _context.SaveChanges();
        return _projectService.GetProjectDto(updatedCaseDto.ProjectId);
    }

    public void CreateAndUpdateExplorationWells(ExplorationWellDto[] explorationWellDtos, Guid caseId)
    {
        var changes = false;
        var runSaveChanges = false;
        foreach (var explorationWellDto in explorationWellDtos)
        {
            if (explorationWellDto.DrillingSchedule?.Values?.Length > 0)
            {
                if (explorationWellDto.DrillingSchedule.Id == Guid.Empty)
                {
                    var explorationWell = ExplorationWellAdapter.Convert(explorationWellDto);
                    _context.ExplorationWell!.Add(explorationWell);
                    changes = true;
                    runSaveChanges = true;
                }
                else
                {
                    if (explorationWellDto.HasChanges)
                    {
                        var existingExplorationWell = _explorationWellService.GetExplorationWell(explorationWellDto.WellId, explorationWellDto.ExplorationId);
                        ExplorationWellAdapter.ConvertExisting(existingExplorationWell, explorationWellDto);
                        if (explorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
                        {
                            _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
                        }

                        _context.ExplorationWell!.Update(existingExplorationWell);
                        changes = true;
                    }
                }
            }
        }

        if (changes)
        {
            if (runSaveChanges)
            {
                _context.SaveChanges();
            }
            var explorationDto = _costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfilesForCase(caseId);
            UpdateExploration(explorationDto);
        }
    }

    public void CreateAndUpdateWellProjectWells(WellProjectWellDto[] wellProjectWellDtos, Guid caseId)
    {
        var changes = false;
        var runSaveChanges = false;
        foreach (var wellProjectWellDto in wellProjectWellDtos)
        {
            if (wellProjectWellDto.DrillingSchedule?.Values?.Length > 0)
            {
                if (wellProjectWellDto.DrillingSchedule.Id == Guid.Empty)
                {
                    var wellProjectWell = WellProjectWellAdapter.Convert(wellProjectWellDto);
                    _context.WellProjectWell!.Add(wellProjectWell);
                    changes = true;
                    runSaveChanges = true;
                }
                else
                {
                    if (wellProjectWellDto.HasChanges)
                    {
                        var existingWellProjectWell = _wellProjectWellService.GetWellProjectWell(wellProjectWellDto.WellId, wellProjectWellDto.WellProjectId);
                        WellProjectWellAdapter.ConvertExisting(existingWellProjectWell, wellProjectWellDto);
                        if (wellProjectWellDto.DrillingSchedule == null && existingWellProjectWell.DrillingSchedule != null)
                        {
                            _context.DrillingSchedule!.Remove(existingWellProjectWell.DrillingSchedule);
                        }

                        _context.WellProjectWell!.Update(existingWellProjectWell);
                        changes = true;
                    }
                }
            }
        }

        if (changes)
        {
            if (runSaveChanges)
            {
                _context.SaveChanges();
            }
            var wellProjectDto = _costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfilesForCase(caseId);
            UpdateWellProject(wellProjectDto);
        }
    }

    public CaseDto UpdateCase(CaseDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _caseService.GetCase(updatedDto.Id);
        CaseAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Cases!.Update(item);
        return CaseDtoAdapter.Convert(updatedItem.Entity);
    }

    public DrainageStrategyDto UpdateDrainageStrategy(DrainageStrategyDto updatedDto, PhysUnit unit)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _drainageStrategyService.GetDrainageStrategy(updatedDto.Id);
        DrainageStrategyAdapter.ConvertExisting(item, updatedDto, unit, false);
        var updatedItem = _context.DrainageStrategies!.Update(item);
        return DrainageStrategyDtoAdapter.Convert(updatedItem.Entity, unit);
    }

    public WellProjectDto UpdateWellProject(WellProjectDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _wellProjectService.GetWellProject(updatedDto.Id);
        WellProjectAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.WellProjects!.Update(item);
        return WellProjectDtoAdapter.Convert(updatedItem.Entity);
    }

    public ExplorationDto UpdateExploration(ExplorationDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _explorationService.GetExploration(updatedDto.Id);
        ExplorationAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Explorations!.Update(item);
        return ExplorationDtoAdapter.Convert(updatedItem.Entity);
    }

    public SurfDto UpdateSurf(SurfDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _surfService.GetSurf(updatedDto.Id);
        SurfAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Surfs!.Update(item);
        return SurfDtoAdapter.Convert(updatedItem.Entity);
    }

    public SubstructureDto UpdateSubstructure(SubstructureDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _substructureService.GetSubstructure(updatedDto.Id);
        SubstructureAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Substructures!.Update(item);
        return SubstructureDtoAdapter.Convert(updatedItem.Entity);
    }

    public TransportDto UpdateTransport(TransportDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _transportService.GetTransport(updatedDto.Id);
        TransportAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Transports!.Update(item);
        return TransportDtoAdapter.Convert(updatedItem.Entity);
    }
    public TopsideDto UpdateTopside(TopsideDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _topsideService.GetTopside(updatedDto.Id);
        TopsideAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Topsides!.Update(item);
        return TopsideDtoAdapter.Convert(updatedItem.Entity);
    }
}
