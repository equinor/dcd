using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ILogger<WellProjectService> _logger;

    public WellProjectService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<WellProjectService>();
    }

    public IEnumerable<WellProject> GetWellProjects(Guid projectId)
    {
        if (_context.WellProjects != null)
        {
            return _context.WellProjects
                .Include(c => c.CostProfile)
                .Include(c => c.OilProducerCostProfile)
                .Include(c => c.GasProducerCostProfile)
                .Include(c => c.WaterInjectorCostProfile)
                .Include(c => c.GasInjectorCostProfile)
                .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<WellProject>();
        }
    }

    public WellProjectDto CopyWellProject(Guid wellProjectId, Guid sourceCaseId)
    {
        var source = GetWellProject(wellProjectId);
        var newWellProjectDto = WellProjectDtoAdapter.Convert(source);
        newWellProjectDto.Id = Guid.Empty;
        if (newWellProjectDto.CostProfile != null)
        {
            newWellProjectDto.CostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.OilProducerCostProfile != null)
        {
            newWellProjectDto.OilProducerCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.GasProducerCostProfile != null)
        {
            newWellProjectDto.GasProducerCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.WaterInjectorCostProfile != null)
        {
            newWellProjectDto.WaterInjectorCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.GasInjectorCostProfile != null)
        {
            newWellProjectDto.GasInjectorCostProfile.Id = Guid.Empty;
        }

        var wellProject = NewCreateWellProject(newWellProjectDto, sourceCaseId);
        var dto = WellProjectDtoAdapter.Convert(wellProject);

        return dto;
    }

    public ProjectDto CreateWellProject(WellProject wellProject, Guid sourceCaseId)
    {
        var project = _projectService.GetProject(wellProject.ProjectId);
        wellProject.Project = project;
        _context.WellProjects!.Add(wellProject);
        _context.SaveChanges();
        SetCaseLink(wellProject, sourceCaseId, project);
        return _projectService.GetProjectDto(project.Id);
    }

    public WellProject NewCreateWellProject(WellProjectDto wellProjectDto, Guid sourceCaseId)
    {
        var wellProject = WellProjectAdapter.Convert(wellProjectDto);
        var project = _projectService.GetProject(wellProject.ProjectId);
        wellProject.Project = project;
        var createdWellProject = _context.WellProjects!.Add(wellProject);
        _context.SaveChanges();
        SetCaseLink(wellProject, sourceCaseId, project);
        return createdWellProject.Entity;
    }

    private void SetCaseLink(WellProject wellProject, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.WellProjectLink = wellProject.Id;
        _context.SaveChanges();
    }

    public ProjectDto DeleteWellProject(Guid wellProjectId)
    {
        _logger.LogWarning("An example of a Warning trace..");
        _logger.LogError("An example of an Error level message");

        var wellProject = GetWellProject(wellProjectId);
        _context.WellProjects!.Remove(wellProject);
        DeleteCaseLinks(wellProjectId);
        _context.SaveChanges();
        return _projectService.GetProjectDto(wellProject.ProjectId);
    }

    private void DeleteCaseLinks(Guid wellProjectId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.WellProjectLink == wellProjectId)
            {
                c.WellProjectLink = Guid.Empty;
            }
        }
    }

    public void CalculateCostProfile(WellProject? wellProject, WellProjectWell wellProjectWell, Well? updatedWell)
    {
        if (wellProject != null && wellProject?.CostProfile?.Override != true)
        {
            var project = _context.Projects!.Include(p => p.Wells).FirstOrDefault(p => p.Id == wellProject!.ProjectId);
            if (wellProject!.WellProjectWells != null && project?.Wells != null
                                                     && (wellProject.WellProjectWells.Any(wpw => wpw.DrillingSchedule != null
                                                         && wpw.WellId != wellProjectWell.WellId) || wellProjectWell.DrillingSchedule != null))
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
                GenerateCostProfileFromDrillingSchedules(wellProject, wellProject.WellProjectWells.ToList(), wells);
                var wellProjectDto = WellProjectDtoAdapter.Convert(wellProject);
                UpdateWellProject(wellProjectDto);
            }
            else if (wellProject.WellProjectWells != null && project?.Wells != null)
            {
                wellProject.CostProfile = null;
                var wellProjectDto = WellProjectDtoAdapter.Convert(wellProject);
                UpdateWellProject(wellProjectDto);
            }
        }
    }

    public void GenerateCostProfileFromDrillingSchedules(WellProject wellProject, List<WellProjectWell> wellProjectWells, List<Well> wells)
    {
        var costProfile = new WellProjectCostProfile();
        var costProfiles = new List<WellProjectCostProfile>();
        foreach (var wellProjectWell in wellProjectWells)
        {
            if (wellProjectWell.DrillingSchedule == null) { continue; }

            var well = wells.Find(w => w.Id == wellProjectWell.WellId);
            if (well == null) { continue; }

            var wellCostProfile = new WellProjectCostProfile();
            wellCostProfile.StartYear = wellProjectWell.DrillingSchedule.StartYear;
            var values = new List<double>();
            foreach (var value in wellProjectWell.DrillingSchedule.Values)
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
        if (wellProject.CostProfile != null)
        {
            wellProject.CostProfile.StartYear = costProfile.StartYear;
            wellProject.CostProfile.Values = costProfile.Values;
        }
        else
        {
            wellProject.CostProfile = costProfile;
        }
    }

    public ProjectDto UpdateWellProject(WellProjectDto updatedWellProject)
    {
        var existing = GetWellProject(updatedWellProject.Id);
        WellProjectAdapter.ConvertExisting(existing, updatedWellProject);

        if (updatedWellProject.CostProfile == null && existing.CostProfile != null)
        {
            _context.WellProjectCostProfile!.Remove(existing.CostProfile);
        }

        _context.WellProjects!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(updatedWellProject.ProjectId);
    }

    public WellProjectDto NewUpdateWellProject(WellProjectDto updatedWellProjectDto)
    {
        var existing = GetWellProject(updatedWellProjectDto.Id);
        WellProjectAdapter.ConvertExisting(existing, updatedWellProjectDto);

        if (updatedWellProjectDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.WellProjectCostProfile!.Remove(existing.CostProfile);
        }

        var updatedWellProject = _context.WellProjects!.Update(existing);
        _context.SaveChanges();
        return WellProjectDtoAdapter.Convert(updatedWellProject.Entity);
    }

    public WellProject GetWellProject(Guid wellProjectId)
    {
        var wellProject = _context.WellProjects!
            .Include(c => c.CostProfile)
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.DrillingSchedule)
            .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.Well)
            .FirstOrDefault(o => o.Id == wellProjectId);
        if (wellProject == null)
        {
            throw new ArgumentException(string.Format("Well project {0} not found.", wellProjectId));
        }
        return wellProject;
    }
}
