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
                .Include(c => c.ExplorationWellCostProfile)
                .Include(c => c.AppraisalWellCostProfile)
                .Include(c => c.SidetrackCostProfile)
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

    public ExplorationDto CopyExploration(Guid explorationId, Guid sourceCaseId)
    {
        var source = GetExploration(explorationId);
        var newExplorationDto = ExplorationDtoAdapter.Convert(source);
        newExplorationDto.Id = Guid.Empty;
        if (newExplorationDto.ExplorationWellCostProfile != null)
        {
            newExplorationDto.ExplorationWellCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.AppraisalWellCostProfile != null)
        {
            newExplorationDto.AppraisalWellCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.SidetrackCostProfile != null)
        {
            newExplorationDto.SidetrackCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.SeismicAcquisitionAndProcessing != null)
        {
            newExplorationDto.SeismicAcquisitionAndProcessing.Id = Guid.Empty;
        }
        if (newExplorationDto.CountryOfficeCost != null)
        {
            newExplorationDto.CountryOfficeCost.Id = Guid.Empty;
        }
        if (newExplorationDto.GAndGAdminCost != null)
        {
            newExplorationDto.GAndGAdminCost.Id = Guid.Empty;
        }

        var wellProject = NewCreateExploration(newExplorationDto, sourceCaseId);
        var dto = ExplorationDtoAdapter.Convert(wellProject);

        return dto;
    }

    public ProjectDto CreateExploration(ExplorationDto explorationDto, Guid sourceCaseId)
    {
        var exploration = ExplorationAdapter.Convert(explorationDto);
        var project = _projectService.GetProjectWithAssets(exploration.ProjectId);
        exploration.Project = project;
        _context.Explorations!.Add(exploration);
        _context.SaveChanges();
        SetCaseLink(exploration, sourceCaseId, project);
        return _projectService.GetProjectDto(exploration.ProjectId);
    }

    public Exploration NewCreateExploration(ExplorationDto explorationDto, Guid sourceCaseId)
    {
        var exploration = ExplorationAdapter.Convert(explorationDto);
        var project = _projectService.GetProjectWithAssets(exploration.ProjectId);
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

    public ProjectDto UpdateExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);

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

    public ExplorationDto[] UpdateMultiple(ExplorationDto[] updatedExplorationDtos)
    {
        var updatedExplorationDtoList = new List<ExplorationDto>();
        foreach (var explorationDto in updatedExplorationDtos)
        {
            var updatedExplorationDto = UpdateSingleExploration(explorationDto);
            updatedExplorationDtoList.Add(updatedExplorationDto);
        }

        _context.SaveChanges();
        return updatedExplorationDtoList.ToArray();
    }

    public ExplorationDto UpdateSingleExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);
        var exploration = _context.Explorations!.Update(existing);
        return ExplorationDtoAdapter.Convert(exploration.Entity);
    }

    public Exploration GetExploration(Guid explorationId)
    {
        var exploration = _context.Explorations!
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
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
