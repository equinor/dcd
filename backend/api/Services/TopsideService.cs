using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TopsideService : ITopsideService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<TopsideService> _logger;

    public TopsideService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TopsideService>();
    }

    public async Task<TopsideDto> CopyTopside(Guid topsideId, Guid sourceCaseId)
    {
        var source = await GetTopside(topsideId);
        var newTopsideDto = TopsideDtoAdapter.Convert(source);
        newTopsideDto.Id = Guid.Empty;
        if (newTopsideDto.CostProfile != null)
        {
            newTopsideDto.CostProfile.Id = Guid.Empty;
        }
        if (newTopsideDto.CostProfileOverride != null)
        {
            newTopsideDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newTopsideDto.CessationCostProfile != null)
        {
            newTopsideDto.CessationCostProfile.Id = Guid.Empty;
        }

        var topside = await NewCreateTopside(newTopsideDto, sourceCaseId);
        var dto = TopsideDtoAdapter.Convert(topside);

        return dto;
    }

    public async Task<ProjectDto> CreateTopside(TopsideDto topsideDto, Guid sourceCaseId)
    {
        var topside = TopsideAdapter.Convert(topsideDto);
        var project = await _projectService.GetProjectAsync(topsideDto.ProjectId);
        topside.Project = project;
        topside.LastChangedDate = DateTimeOffset.UtcNow;
        topside.ProspVersion = topsideDto.ProspVersion;
        _context.Topsides!.Add(topside);
        await _context.SaveChangesAsync();
        await SetCaseLink(topside, sourceCaseId, project);
        return await _projectService.GetProjectDtoAsync(project.Id);
    }

    public async Task<Topside> NewCreateTopside(TopsideDto topsideDto, Guid sourceCaseId)
    {
        var topside = TopsideAdapter.Convert(topsideDto);
        var project = await _projectService.GetProjectAsync(topsideDto.ProjectId);
        topside.Project = project;
        topside.LastChangedDate = DateTimeOffset.UtcNow;
        topside.ProspVersion = topsideDto.ProspVersion;
        var createdTopside = _context.Topsides!.Add(topside);
        await _context.SaveChangesAsync();
        await SetCaseLink(topside, sourceCaseId, project);
        return createdTopside.Entity;
    }

    private async Task SetCaseLink(Topside topside, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.TopsideLink = topside.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectDto> DeleteTopside(Guid topsideId)
    {
        var topside = await GetTopside(topsideId);
        _context.Topsides!.Remove(topside);
        DeleteCaseLinks(topsideId);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDtoAsync(topside.ProjectId);
    }

    private void DeleteCaseLinks(Guid topsideId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.TopsideLink == topsideId)
            {
                c.TopsideLink = Guid.Empty;
            }
        }
    }

    public async Task<ProjectDto> UpdateTopside(TopsideDto updatedTopsideDto)
    {
        var existing = await GetTopside(updatedTopsideDto.Id);
        TopsideAdapter.ConvertExisting(existing, updatedTopsideDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Topsides!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDtoAsync(updatedTopsideDto.ProjectId);
    }

    public async Task<TopsideDto> NewUpdateTopside(TopsideDto updatedTopsideDto)
    {
        var existing = await GetTopside(updatedTopsideDto.Id);
        TopsideAdapter.ConvertExisting(existing, updatedTopsideDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        var updatedTopside = _context.Topsides!.Update(existing);
        await _context.SaveChangesAsync();
        return TopsideDtoAdapter.Convert(updatedTopside.Entity);
    }

    public async Task<Topside> GetTopside(Guid topsideId)
    {
        var topside = await _context.Topsides!
            .Include(c => c.Project)
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == topsideId);
        if (topside == null)
        {
            throw new ArgumentException(string.Format("Topside {0} not found.", topsideId));
        }
        return topside;
    }
}
