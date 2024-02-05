using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfService : ISurfService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    public SurfService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
    }

    public async Task<SurfDto> CopySurf(Guid surfId, Guid sourceCaseId)
    {
        var source = await GetSurf(surfId);
        var newSurfDto = SurfDtoAdapter.Convert(source);
        newSurfDto.Id = Guid.Empty;
        if (newSurfDto.CostProfile != null)
        {
            newSurfDto.CostProfile.Id = Guid.Empty;
        }
        if (newSurfDto.CostProfileOverride != null)
        {
            newSurfDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newSurfDto.CessationCostProfile != null)
        {
            newSurfDto.CessationCostProfile.Id = Guid.Empty;
        }

        var surf = await NewCreateSurf(newSurfDto, sourceCaseId);
        var dto = SurfDtoAdapter.Convert(surf);

        return dto;
    }

    public async Task<ProjectDto> UpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = await GetSurf(updatedSurfDto.Id);
        SurfAdapter.ConvertExisting(existing, updatedSurfDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDtoAsync(existing.ProjectId);
    }

    public async Task<SurfDto> NewUpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = await GetSurf(updatedSurfDto.Id);
        SurfAdapter.ConvertExisting(existing, updatedSurfDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        var updatedSurf = _context.Surfs!.Update(existing);
        await _context.SaveChangesAsync();
        return SurfDtoAdapter.Convert(updatedSurf.Entity);
    }

    public async Task<Surf> GetSurf(Guid surfId)
    {
        var surf = await _context.Surfs!
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == surfId);
        if (surf == null)
        {
            throw new ArgumentException(string.Format("Surf {0} not found.", surfId));
        }
        return surf;
    }

    public async Task<ProjectDto> CreateSurf(SurfDto surfDto, Guid sourceCaseId)
    {
        var surf = SurfAdapter.Convert(surfDto);
        var project = await _projectService.GetProjectAsync(surf.ProjectId);
        surf.Project = project;
        surf.ProspVersion = surfDto.ProspVersion;
        surf.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Add(surf);
        await _context.SaveChangesAsync();
        await SetCaseLink(surf, sourceCaseId, project);
        return await _projectService.GetProjectDtoAsync(surf.ProjectId);
    }

    public async Task<Surf> NewCreateSurf(SurfDto surfDto, Guid sourceCaseId)
    {
        var surf = SurfAdapter.Convert(surfDto);
        var project = await _projectService.GetProjectAsync(surf.ProjectId);
        surf.Project = project;
        surf.LastChangedDate = DateTimeOffset.UtcNow;
        var createdSurf = _context.Surfs!.Add(surf);
        await _context.SaveChangesAsync();
        await SetCaseLink(surf, sourceCaseId, project);
        return createdSurf.Entity;
    }

    private async Task SetCaseLink(Surf surf, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.SurfLink = surf.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectDto> DeleteSurf(Guid surfId)
    {
        var surf = await GetSurf(surfId);
        _context.Surfs!.Remove(surf);
        await DeleteCaseLinks(surfId);
        return await _projectService.GetProjectDtoAsync(surf.ProjectId);
    }

    private async Task DeleteCaseLinks(Guid surfId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.SurfLink == surfId)
            {
                c.SurfLink = Guid.Empty;
            }
        }
        await _context.SaveChangesAsync();
    }
}
