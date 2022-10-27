using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;

    private readonly ILogger<ExplorationService> _logger;

    public ExplorationService(DcdDbContext context, ProjectService
        projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
    }

    public IEnumerable<Exploration> GetExplorations(Guid projectId)
    {
        if (_context.Explorations != null)
        {
            return _context.Explorations
                .Include(c => c.CostProfile)
                .Include(c => c.GAndGAdminCost)
                .Include(c => c.SeismicAcquisitionAndProcessing)
                .Include(c => c.CountryOfficeCost)
                .Include(c => c.ExplorationWells!).ThenInclude(ew => ew.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<Exploration>();
        }
    }

    public ProjectDto CreateExploration(ExplorationDto explorationDto, Guid sourceCaseId)
    {
        var exploration = ExplorationAdapter.Convert(explorationDto);
        var project = _projectService.GetProject(exploration.ProjectId);
        exploration.Project = project;
        _context.Explorations!.Add(exploration);
        _context.SaveChanges();
        SetCaseLink(exploration, sourceCaseId, project);
        return _projectService.GetProjectDto(exploration.ProjectId);
    }

    public Exploration NewCreateExploration(ExplorationDto explorationDto, Guid sourceCaseId)
    {
        var exploration = ExplorationAdapter.Convert(explorationDto);
        var project = _projectService.GetProject(exploration.ProjectId);
        exploration.Project = project;
        var createdExploration = _context.Explorations!.Add(exploration);
        _context.SaveChanges();
        SetCaseLink(exploration, sourceCaseId, project);
        return createdExploration.Entity;
    }

    private void SetCaseLink(Exploration exploration, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.ExplorationLink = exploration.Id;
        _context.SaveChanges();
    }

    public ProjectDto DeleteExploration(Guid explorationId)
    {
        var exploration = GetExploration(explorationId);
        _context.Explorations!.Remove(exploration);
        DeleteCaseLinks(explorationId);
        _context.SaveChanges();
        return _projectService.GetProjectDto(exploration.ProjectId);
    }

    private void DeleteCaseLinks(Guid explorationId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.ExplorationLink == explorationId)
            {
                c.ExplorationLink = Guid.Empty;
            }
        }
    }

    public void CalculateCostProfile(Exploration? exploration, ExplorationWell explorationWell, Well? updatedWell)
    {
        if (exploration != null && exploration?.CostProfile?.Override != true)
        {
            var project = _context.Projects!.Include(p => p.Wells).FirstOrDefault(p => p.Id == exploration!.ProjectId);
            if (exploration!.ExplorationWells != null && project?.Wells != null
                                                     && (exploration.ExplorationWells.Any(wpw => wpw.DrillingSchedule != null
                                                         && wpw.WellId != explorationWell.WellId) || explorationWell.DrillingSchedule != null))
            {
                var wells = project.Wells.ToList();
                if (updatedWell != null)
                {
                    var index = wells.FindIndex(w => w.Id == updatedWell.Id);
                    if (index >= 0)
                    {
                        wells[index].WellCost = updatedWell.WellCost;
                    }
                }
                GenerateCostProfileFromDrillingSchedules(exploration, exploration.ExplorationWells.ToList(), wells);
                var explorationDto = ExplorationDtoAdapter.Convert(exploration);
                UpdateExploration(explorationDto);
            }
            else if (exploration.ExplorationWells != null && project?.Wells != null)
            {
                exploration.CostProfile = null;
                var explorationDto = ExplorationDtoAdapter.Convert(exploration);
                UpdateExploration(explorationDto);
            }
        }
    }

    public void GenerateCostProfileFromDrillingSchedules(Exploration exploration, List<ExplorationWell> explorationWells, List<Well> wells)
    {
        var costProfile = new ExplorationCostProfile();
        var costProfiles = new List<ExplorationCostProfile>();
        foreach (var explorationWell in explorationWells)
        {
            if (explorationWell.DrillingSchedule == null) { continue; }

            var well = wells.Find(w => w.Id == explorationWell.WellId);
            if (well == null) { continue; }

            var wellCostProfile = new ExplorationCostProfile();
            wellCostProfile.StartYear = explorationWell.DrillingSchedule.StartYear;
            var values = new List<double>();
            foreach (var value in explorationWell.DrillingSchedule.Values)
            {
                values.Add(value * well.WellCost);
            }
            wellCostProfile.Values = values.ToArray();
            costProfiles.Add(wellCostProfile);
        }

        var tempCostProfile = new TimeSeries<double>();
        foreach (var profile in costProfiles)
        {
            tempCostProfile = TimeSeriesCost.MergeCostProfiles(tempCostProfile, profile);
        }

        costProfile.StartYear = tempCostProfile.StartYear;
        costProfile.Values = tempCostProfile.Values;
        if (exploration.CostProfile != null)
        {
            exploration.CostProfile.StartYear = costProfile.StartYear;
            exploration.CostProfile.Values = costProfile.Values;
        }
        else
        {
            exploration.CostProfile = costProfile;
        }
    }

    public ProjectDto UpdateExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);

        if (updatedExplorationDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.ExplorationCostProfile!.Remove(existing.CostProfile);
        }

        if (updatedExplorationDto.GAndGAdminCost == null && existing.GAndGAdminCost != null)
        {
            _context.GAndGAdminCost!.Remove(existing.GAndGAdminCost);
        }

        if (updatedExplorationDto.SeismicAcquisitionAndProcessing == null && existing.SeismicAcquisitionAndProcessing != null)
        {
            _context.SeismicAcquisitionAndProcessing!.Remove(existing.SeismicAcquisitionAndProcessing);
        }

        if (updatedExplorationDto.CountryOfficeCost == null && existing.CountryOfficeCost != null)
        {
            _context.CountryOfficeCost!.Remove(existing.CountryOfficeCost);
        }

        _context.Explorations!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(existing.ProjectId);
    }

    public ExplorationDto NewUpdateExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);

        if (updatedExplorationDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.ExplorationCostProfile!.Remove(existing.CostProfile);
        }

        if (updatedExplorationDto.GAndGAdminCost == null && existing.GAndGAdminCost != null)
        {
            _context.GAndGAdminCost!.Remove(existing.GAndGAdminCost);
        }

        if (updatedExplorationDto.SeismicAcquisitionAndProcessing == null && existing.SeismicAcquisitionAndProcessing != null)
        {
            _context.SeismicAcquisitionAndProcessing!.Remove(existing.SeismicAcquisitionAndProcessing);
        }

        if (updatedExplorationDto.CountryOfficeCost == null && existing.CountryOfficeCost != null)
        {
            _context.CountryOfficeCost!.Remove(existing.CountryOfficeCost);
        }

        var updatedExploration = _context.Explorations!.Update(existing);
        _context.SaveChanges();
        return ExplorationDtoAdapter.Convert(updatedExploration.Entity);
    }

    public Exploration GetExploration(Guid explorationId)
    {

        var exploration = _context.Explorations!
            .Include(c => c.CostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .Include(c => c.ExplorationWells!).ThenInclude(ew => ew.DrillingSchedule)
            .Include(ew => ew.ExplorationWells!).ThenInclude(ew => ew.Well)
            .FirstOrDefault(o => o.Id == explorationId);
        if (exploration == null)
        {
            throw new ArgumentException(string.Format("Exploration {0} not found.", explorationId));
        }
        return exploration;
    }
}
