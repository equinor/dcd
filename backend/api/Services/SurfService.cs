using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfService
{
    private readonly DcdDbContext _context;
    private readonly ILogger<SurfService> _logger;
    private readonly ProjectService _projectService;

    public SurfService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
    }

    public IEnumerable<Surf> GetSurfs(Guid projectId)
    {
        if (_context.Surfs != null)
        {
            return _context.Surfs
                .Include(c => c.CostProfile)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }

        return new List<Surf>();
    }

    public ProjectDto UpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = GetSurf(updatedSurfDto.Id).Result;
        SurfAdapter.ConvertExisting(existing, updatedSurfDto);

        if (updatedSurfDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.SurfCostProfile!.Remove(existing.CostProfile);
        }

        if (updatedSurfDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.SurfCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        _context.Surfs!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(existing.ProjectId);
    }

    public SurfDto NewUpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = GetSurf(updatedSurfDto.Id).Result;
        SurfAdapter.ConvertExisting(existing, updatedSurfDto);

        if (updatedSurfDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.SurfCostProfile!.Remove(existing.CostProfile);
        }

        if (updatedSurfDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.SurfCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        var updatedSurf = _context.Surfs!.Update(existing);
        _context.SaveChanges();
        return SurfDtoAdapter.Convert(updatedSurf.Entity);
    }

    public async Task<Surf> GetSurf(Guid surfId)
    {
        var surf = await _context.Surfs!
            .Include(c => c.CostProfile)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == surfId);
        if (surf == null)
        {
            throw new ArgumentException($"Surf {surfId} not found.");
        }

        return surf;
    }

    public async Task<ProjectDto> CreateSurf(SurfDto surfDto, Guid sourceCaseId)
    {
        var surf = SurfAdapter.Convert(surfDto);
        var project = _projectService.GetProject(surf.ProjectId);
        surf.Project = project;
        surf.ProspVersion = surfDto.ProspVersion;
        surf.LastChangedDate = DateTimeOffset.Now;
        _context.Surfs!.Add(surf);
        await _context.SaveChangesAsync();
        SetCaseLink(surf, sourceCaseId, project);
        return _projectService.GetProjectDto(surf.ProjectId);
    }

    public async Task<Surf> NewCreateSurf(SurfDto surfDto, Guid sourceCaseId)
    {
        var surf = SurfAdapter.Convert(surfDto);
        var project = _projectService.GetProject(surf.ProjectId);
        surf.Project = project;
        surf.LastChangedDate = DateTimeOffset.Now;
        var createdSurf = _context.Surfs!.Add(surf);
        await _context.SaveChangesAsync();
        SetCaseLink(surf, sourceCaseId, project);
        return createdSurf.Entity;
    }

    private void SetCaseLink(Surf surf, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }

        case_.SurfLink = surf.Id;
        _context.SaveChanges();
    }

    public async Task<ProjectDto> DeleteSurf(Guid surfId)
    {
        var surf = await GetSurf(surfId);
        _context.Surfs!.Remove(surf);
        DeleteCaseLinks(surfId);
        return _projectService.GetProjectDto(surf.ProjectId);
    }

    private void DeleteCaseLinks(Guid surfId)
    {
        foreach (var c in _context.Cases!)
        {
            if (c.SurfLink == surfId)
            {
                c.SurfLink = Guid.Empty;
            }
        }

        _context.SaveChanges();
    }
}
