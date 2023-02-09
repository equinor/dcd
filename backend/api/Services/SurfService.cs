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

    public SurfDto CopySurf(Guid surfId, Guid sourceCaseId)
    {
        var source = GetSurf(surfId);
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

        var surf = NewCreateSurf(newSurfDto, sourceCaseId);
        var dto = SurfDtoAdapter.Convert(surf);

        return dto;
    }

    public ProjectDto UpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = GetSurf(updatedSurfDto.Id);
        SurfAdapter.ConvertExisting(existing, updatedSurfDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(existing.ProjectId);
    }

    public SurfDto NewUpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = GetSurf(updatedSurfDto.Id);
        SurfAdapter.ConvertExisting(existing, updatedSurfDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        var updatedSurf = _context.Surfs!.Update(existing);
        _context.SaveChanges();
        return SurfDtoAdapter.Convert(updatedSurf.Entity);
    }

    public Surf GetSurf(Guid surfId)
    {
        var surf = _context.Surfs!
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefault(o => o.Id == surfId);
        if (surf == null)
        {
            throw new ArgumentException(string.Format("Surf {0} not found.", surfId));
        }
        return surf;
    }

    public ProjectDto CreateSurf(SurfDto surfDto, Guid sourceCaseId)
    {
        var surf = SurfAdapter.Convert(surfDto);
        var project = _projectService.GetProject(surf.ProjectId);
        surf.Project = project;
        surf.ProspVersion = surfDto.ProspVersion;
        surf.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Add(surf);
        _context.SaveChanges();
        SetCaseLink(surf, sourceCaseId, project);
        return _projectService.GetProjectDto(surf.ProjectId);
    }

    public Surf NewCreateSurf(SurfDto surfDto, Guid sourceCaseId)
    {
        var surf = SurfAdapter.Convert(surfDto);
        var project = _projectService.GetProject(surf.ProjectId);
        surf.Project = project;
        surf.LastChangedDate = DateTimeOffset.UtcNow;
        var createdSurf = _context.Surfs!.Add(surf);
        _context.SaveChanges();
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

    public ProjectDto DeleteSurf(Guid surfId)
    {
        var surf = GetSurf(surfId);
        _context.Surfs!.Remove(surf);
        DeleteCaseLinks(surfId);
        return _projectService.GetProjectDto(surf.ProjectId);
    }

    private void DeleteCaseLinks(Guid surfId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.SurfLink == surfId)
            {
                c.SurfLink = Guid.Empty;
            }
        }
        _context.SaveChanges();
    }
}
