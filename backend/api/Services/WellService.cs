using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly IServiceProvider _serviceProvider;
    private readonly WellProjectService _wellProjectService;
    private readonly ExplorationService _explorationService;
    private readonly ILogger<CaseService> _logger;

    public WellService(DcdDbContext context, ProjectService projectService, WellProjectService wellProjectService, ExplorationService explorationService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
        _serviceProvider = serviceProvider;
        _wellProjectService = wellProjectService;
        _explorationService = explorationService;
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

    public WellDto[]? UpdateMultipleWells(WellDto[] updatedWellDtos)
    {
        ProjectDto? projectDto = null;
        foreach (var wellDto in updatedWellDtos)
        {
            projectDto = UpdateWell(wellDto);
        }

        var costProfileHelper = _serviceProvider.GetRequiredService<CostProfileFromDrillingScheduleHelper>();
        costProfileHelper.UpdateCostProfilesForWells(updatedWellDtos.Select(w => w.Id).ToList());

        if (projectDto != null)
        {
            return projectDto.Wells?.ToArray();
        }
        return null;
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
