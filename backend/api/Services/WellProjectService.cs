using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectService : IWellProjectService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<WellProjectService> _logger;
    private readonly IMapper _mapper;

    public WellProjectService(
        DcdDbContext context, 
        IProjectService projectService, 
        ILoggerFactory loggerFactory,
        IMapper mapper
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<WellProjectService>();
        _mapper = mapper;
    }

    public async Task<WellProjectDto> CopyWellProject(Guid wellProjectId, Guid sourceCaseId)
    {
        var source = await GetWellProject(wellProjectId);
        var newWellProjectDto = WellProjectDtoAdapter.Convert(source);
        newWellProjectDto.Id = Guid.Empty;

        if (newWellProjectDto.OilProducerCostProfile != null)
        {
            newWellProjectDto.OilProducerCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.OilProducerCostProfileOverride != null)
        {
            newWellProjectDto.OilProducerCostProfileOverride.Id = Guid.Empty;
        }

        if (newWellProjectDto.GasProducerCostProfile != null)
        {
            newWellProjectDto.GasProducerCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.GasProducerCostProfileOverride != null)
        {
            newWellProjectDto.GasProducerCostProfileOverride.Id = Guid.Empty;
        }

        if (newWellProjectDto.WaterInjectorCostProfile != null)
        {
            newWellProjectDto.WaterInjectorCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.WaterInjectorCostProfileOverride != null)
        {
            newWellProjectDto.WaterInjectorCostProfileOverride.Id = Guid.Empty;
        }

        if (newWellProjectDto.GasInjectorCostProfile != null)
        {
            newWellProjectDto.GasInjectorCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.GasInjectorCostProfileOverride != null)
        {
            newWellProjectDto.GasInjectorCostProfileOverride.Id = Guid.Empty;
        }

        // var wellProject = await NewCreateWellProject(newWellProjectDto, sourceCaseId);
        // var dto = WellProjectDtoAdapter.Convert(wellProject);
        // return dto;
        return newWellProjectDto;
    }

    public async Task<WellProject> NewCreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto)
    {
        var wellProject = _mapper.Map<WellProject>(wellProjectDto);
        if (wellProject == null)
        {
            throw new ArgumentNullException(nameof(wellProject));
        }
        var project = await _projectService.GetProject(projectId);
        wellProject.Project = project;
        var createdWellProject = _context.WellProjects!.Add(wellProject);
        await _context.SaveChangesAsync();
        await SetCaseLink(wellProject, sourceCaseId, project);
        return createdWellProject.Entity;
    }

    private async Task SetCaseLink(WellProject wellProject, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.WellProjectLink = wellProject.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<WellProjectDto> NewUpdateWellProject(WellProjectDto updatedWellProjectDto)
    {
        var existing = await GetWellProject(updatedWellProjectDto.Id);
        WellProjectAdapter.ConvertExisting(existing, updatedWellProjectDto);

        var updatedWellProject = _context.WellProjects!.Update(existing);
        await _context.SaveChangesAsync();
        return WellProjectDtoAdapter.Convert(updatedWellProject.Entity);
    }

    public async Task<WellProjectDto[]> UpdateMultiple(WellProjectDto[] updatedWellProjectDtos)
    {
        var updatedWellProjectDtoList = new List<WellProjectDto>();
        foreach (var wellProjectDto in updatedWellProjectDtos)
        {
            var updatedWellProjectDto = await UpdateSingleWellProject(wellProjectDto);
            updatedWellProjectDtoList.Add(updatedWellProjectDto);
        }

        await _context.SaveChangesAsync();
        return updatedWellProjectDtoList.ToArray();
    }

    public async Task<WellProjectDto> UpdateSingleWellProject(WellProjectDto updatedWellProjectDto)
    {
        var existing = await GetWellProject(updatedWellProjectDto.Id);
        WellProjectAdapter.ConvertExisting(existing, updatedWellProjectDto);
        var wellProject = _context.WellProjects!.Update(existing);
        await _context.SaveChangesAsync();
        return WellProjectDtoAdapter.Convert(wellProject.Entity);
    }

    public async Task<WellProject> GetWellProject(Guid wellProjectId)
    {
        var wellProject = await _context.WellProjects!
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.DrillingSchedule)
            .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.Well)
            .FirstOrDefaultAsync(o => o.Id == wellProjectId);
        if (wellProject == null)
        {
            throw new ArgumentException(string.Format("Well project {0} not found.", wellProjectId));
        }
        return wellProject;
    }
}
