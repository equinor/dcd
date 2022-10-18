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
                .Include(c => c.WellProjectWells).ThenInclude(wpw => wpw.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<WellProject>();
        }
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
            var project = _context.Projects!.Include(p => p.Wells).FirstOrDefault(p => p.Id == wellProject.ProjectId);
            if (wellProject.WellProjectWells != null && project?.Wells != null
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

    private static WellProjectCostProfile? MergeCostProfiles(WellProjectCostProfile t1, WellProjectCostProfile t2)
    {
        var t1Year = t1.StartYear;
        var t2Year = t2.StartYear;
        var t1Values = t1.Values;
        var t2Values = t2.Values;
        if (t1Values.Length == 0)
        {
            if (t2.Values.Length == 0)
            {
                return null;
            }
            return t2;
        }
        if (t2Values.Length == 0)
        {
            return t1;
        }

        var offset = t1Year < t2Year ? t2Year - t1Year : t1Year - t2Year;

        var values = new List<double>();
        if (t1Year < t2Year)
        {
            values = MergeTimeSeries(t1Values.ToList(), t2Values.ToList(), offset);
        }
        else
        {
            values = MergeTimeSeries(t2Values.ToList(), t1Values.ToList(), offset);
        }

        var timeSeries = new WellProjectCostProfile();
        timeSeries.StartYear = Math.Min(t1Year, t2Year);
        timeSeries.Values = values.ToArray();
        return timeSeries;
    }

    private static List<double> MergeTimeSeries(List<double> t1, List<double> t2, int offset)
    {
        var doubleList = new List<double>();
        if (offset > t1.Count)
        {
            doubleList.AddRange(t1);
            var zeros = offset - t1.Count;
            var zeroList = Enumerable.Repeat(0.0, zeros);
            doubleList.AddRange(zeroList);
            doubleList.AddRange(t2);
            return doubleList;
        }
        doubleList.AddRange(t1.Take(offset));
        if (t1.Count - offset == t2.Count)
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
        }
        else if (t1.Count - offset > t2.Count)
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            var remaining = t1.Count - offset - t2.Count;
            doubleList.AddRange(t1.TakeLast(remaining));
        }
        else
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            var remaining = t2.Count - (t1.Count - offset);
            doubleList.AddRange(t2.TakeLast(remaining));
        }
        return doubleList;
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

        var tempCostProfile = new WellProjectCostProfile();
        foreach (var profile in costProfiles)
        {
            tempCostProfile = MergeCostProfiles(tempCostProfile, profile);
        }

        costProfile = tempCostProfile;
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

    public WellProject GetWellProject(Guid wellProjectId)
    {
        var wellProject = _context.WellProjects!
            .Include(c => c.CostProfile)
            .Include(c => c.WellProjectWells).ThenInclude(wpw => wpw.DrillingSchedule)
            .Include(c => c.WellProjectWells).ThenInclude(wpw => wpw.Well)
            .FirstOrDefault(o => o.Id == wellProjectId);
        if (wellProject == null)
        {
            throw new ArgumentException(string.Format("Well project {0} not found.", wellProjectId));
        }
        return wellProject;
    }
}
