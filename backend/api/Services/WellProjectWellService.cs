using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectWellService : IWellProjectWellService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper;
    private readonly IWellProjectService wellProjectService;
    private readonly ILogger<CaseService> _logger;

    public WellProjectWellService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper, IWellProjectService wellProjectService)
    {
        _context = context;
        _projectService = projectService;
        this.costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;
        this.wellProjectService = wellProjectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto CreateWellProjectWell(WellProjectWellDto wellProjectWellDto)
    {
        var wellProjectWell = WellProjectWellAdapter.Convert(wellProjectWellDto);
        _context.WellProjectWell!.Add(wellProjectWell);
        _context.SaveChanges();
        var projectId = _context.WellProjects!.FirstOrDefault(c => c.Id == wellProjectWellDto.WellProjectId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public WellProjectWellDto[]? CreateMultipleWellProjectWells(WellProjectWellDto[] wellProjectWellDtos)
    {
        var wellProjectId = wellProjectWellDtos.FirstOrDefault()?.WellProjectId;
        ProjectDto? projectDto = null;
        foreach (var wellProjectWellDto in wellProjectWellDtos)
        {
            projectDto = CreateWellProjectWell(wellProjectWellDto);
        }
        if (projectDto != null && wellProjectId != null)
        {
            return projectDto.WellProjects?.FirstOrDefault(e => e.Id == wellProjectId)?.WellProjectWells?.ToArray();
        }
        return null;
    }

    public ProjectDto UpdateWellProjectWell(WellProjectWellDto updatedWellProjectWellDto)
    {
        var existing = GetWellProjectWell(updatedWellProjectWellDto.WellId, updatedWellProjectWellDto.WellProjectId);
        WellProjectWellAdapter.ConvertExisting(existing, updatedWellProjectWellDto);
        if (updatedWellProjectWellDto.DrillingSchedule == null && existing.DrillingSchedule != null)
        {
            _context.DrillingSchedule!.Remove(existing.DrillingSchedule);
        }

        _context.WellProjectWell!.Update(existing);
        _context.SaveChanges();
        var projectId = _context.WellProjects!.FirstOrDefault(c => c.Id == updatedWellProjectWellDto.WellProjectId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public WellProjectWellDto[]? UpdateMultipleWellProjectWells(WellProjectWellDto[] updatedWellProjectWellDtos, Guid caseId)
    {
        var wellProjectId = updatedWellProjectWellDtos.FirstOrDefault()?.WellProjectId;
        ProjectDto? projectDto = null;
        foreach (var wellProjectWellDto in updatedWellProjectWellDtos)
        {
            projectDto = UpdateWellProjectWell(wellProjectWellDto);
        }

        var wellProjectDto = costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfilesForCase(caseId);

        wellProjectService.NewUpdateWellProject(wellProjectDto);

        if (projectDto != null && wellProjectId != null)
        {
            return projectDto.WellProjects?.FirstOrDefault(e => e.Id == wellProjectId)?.WellProjectWells?.ToArray();
        }
        return null;
    }

    public WellProjectWell GetWellProjectWell(Guid wellId, Guid caseId)
    {
        var wellProjectWell = _context.WellProjectWell!
            .Include(wpw => wpw.DrillingSchedule)
            .FirstOrDefault(w => w.WellId == wellId && w.WellProjectId == caseId);
        if (wellProjectWell == null)
        {
            throw new ArgumentException(string.Format("WellProjectWell {0} not found.", wellId));
        }
        return wellProjectWell;
    }

    public WellProjectWellDto[]? CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId)
    {
        var sourceWellProjectWells = GetAll().Where(wpw => wpw.WellProjectId == sourceWellProjectId).ToList();
        if (sourceWellProjectWells?.Count > 0)
        {
            var newWellProjectWellDtos = new List<WellProjectWellDto>();
            foreach (var wellProjectWell in sourceWellProjectWells)
            {
                var newWellProjectWellDto = WellProjectWellDtoAdapter.Convert(wellProjectWell);
                if (newWellProjectWellDto.DrillingSchedule != null)
                {
                    newWellProjectWellDto.DrillingSchedule.Id = Guid.Empty;
                }
                newWellProjectWellDto.WellProjectId = targetWellProjectId;
                newWellProjectWellDtos.Add(newWellProjectWellDto);
            }
            var result = CreateMultipleWellProjectWells(newWellProjectWellDtos.ToArray());
            return result;
        }
        return null;
    }

    public WellProjectWellDto GetWellProjectWellDto(Guid wellId, Guid caseId)
    {
        var wellProjectWell = GetWellProjectWell(wellId, caseId);
        var wellProjectWellDto = WellProjectWellDtoAdapter.Convert(wellProjectWell);

        return wellProjectWellDto;
    }

    public IEnumerable<WellProjectWell> GetAll()
    {
        if (_context.WellProjectWell != null)
        {
            return _context.WellProjectWell.Include(wpw => wpw.DrillingSchedule);
        }
        else
        {
            _logger.LogInformation("No WellProjectWells existing");
            return new List<WellProjectWell>();
        }
    }

    public IEnumerable<WellProjectWellDto> GetAllDtos()
    {
        var wellProjectWells = GetAll();
        if (wellProjectWells.Any())
        {
            var wellProjectWellDtos = new List<WellProjectWellDto>();
            foreach (WellProjectWell wellProjectWell in wellProjectWells)
            {
                var wellProjectWellDto = WellProjectWellDtoAdapter.Convert(wellProjectWell);
                wellProjectWellDtos.Add(wellProjectWellDto);
            }

            return wellProjectWellDtos;
        }
        else
        {
            return new List<WellProjectWellDto>();
        }
    }
}
