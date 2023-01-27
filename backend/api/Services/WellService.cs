using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellService : IWellService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<WellService> _logger;

    public WellService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper)
    {
        _context = context;
        _projectService = projectService;
        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;
        _logger = loggerFactory.CreateLogger<WellService>();
    }

    public ProjectDto CreateWell(WellDto wellDto)
    {
        var _well = WellAdapter.Convert(wellDto);
        _context.Wells!.Add(_well);
        _context.SaveChanges();
        return _projectService.GetProjectDto(wellDto.ProjectId);
    }

    public ProjectDto UpdateWell(WellDto updatedWellDto)
    {
        var existing = GetWell(updatedWellDto.Id);
        WellAdapter.ConvertExisting(existing, updatedWellDto);

        _context.Wells!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(existing.ProjectId);
    }

    public WellDto UpdateExistingWell(WellDto updatedWellDto)
    {
        var existing = GetWell(updatedWellDto.Id);
        WellAdapter.ConvertExisting(existing, updatedWellDto);

        var well = _context.Wells!.Update(existing);
        return WellDtoAdapter.Convert(well.Entity);
    }

    public WellDto[] UpdateMultipleWells(WellDto[] updatedWellDtos)
    {
        var updatedWellDtoList = new List<WellDto>();
        foreach (var wellDto in updatedWellDtos)
        {
            var updatedWellDto = UpdateExistingWell(wellDto);
            updatedWellDtoList.Add(updatedWellDto);
        }

        _context.SaveChanges();

        _costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWellDtos.Select(w => w.Id).ToList());

        return updatedWellDtoList.ToArray();
    }

    public WellDto[]? CreateMultipleWells(WellDto[] wellDtos)
    {
        ProjectDto? projectDto = null;
        foreach (var wellDto in wellDtos)
        {
            projectDto = CreateWell(wellDto);
        }
        if (projectDto != null)
        {
            return projectDto.Wells?.ToArray();
        }
        return null;
    }

    public Well GetWell(Guid wellId)
    {
        var well = _context.Wells!
            .Include(e => e.WellProjectWells)
            .Include(e => e.ExplorationWells)
            .FirstOrDefault(w => w.Id == wellId);
        if (well == null)
        {
            throw new ArgumentException(string.Format("Well {0} not found.", wellId));
        }
        return well;
    }

    public WellDto GetWellDto(Guid wellId)
    {
        var well = GetWell(wellId);
        var wellDto = WellDtoAdapter.Convert(well);

        return wellDto;
    }

    public IEnumerable<Well> GetAll()
    {
        if (_context.Wells != null)
        {
            return _context.Wells;
        }
        else
        {
            _logger.LogInformation("No Wells existing");
            return new List<Well>();
        }
    }

    public IEnumerable<WellDto> GetDtosForProject(Guid projectId)
    {
        var wells = GetWells(projectId);
        var wellsDtos = new List<WellDto>();
        foreach (Well well in wells)
        {
            wellsDtos.Add(WellDtoAdapter.Convert(well));
        }
        return wellsDtos;
    }

    public IEnumerable<Well> GetWells(Guid projectId)
    {
        if (_context.Wells != null)
        {
            return _context.Wells
                .Where(d => d.ProjectId.Equals(projectId));
        }
        else
        {
            return new List<Well>();
        }
    }

    public IEnumerable<WellDto> GetAllDtos()
    {
        if (GetAll().Any())
        {
            var wells = GetAll();
            var wellDtos = new List<WellDto>();
            foreach (Well well in wells)
            {
                var wellDto = WellDtoAdapter.Convert(well);
                wellDtos.Add(wellDto);
            }

            return wellDtos;
        }
        else
        {
            return new List<WellDto>();
        }
    }
}
