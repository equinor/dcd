using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TopsideService
{
    private readonly DcdDbContext _context;
    private readonly ILogger<TopsideService> _logger;
    private readonly ProjectService _projectService;

    public TopsideService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TopsideService>();
    }

    public IEnumerable<Topside> GetTopsides(Guid projectId)
    {
        if (_context.Topsides != null)
        {
            return _context.Topsides
                .Include(c => c.CostProfile)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }

        return new List<Topside>();
    }

    public async Task<ProjectDto> CreateTopside(TopsideDto topsideDto, Guid sourceCaseId)
    {
        var topside = TopsideAdapter.Convert(topsideDto);
        var project = _projectService.GetProject(topsideDto.ProjectId);
        topside.Project = project;
        topside.LastChangedDate = DateTimeOffset.Now;
        topside.ProspVersion = topsideDto.ProspVersion;
        _context.Topsides!.Add(topside);
        await _context.SaveChangesAsync();
        SetCaseLink(topside, sourceCaseId, project);
        return _projectService.GetProjectDto(project.Id);
    }

    public async Task<Topside> NewCreateTopside(TopsideDto topsideDto, Guid sourceCaseId)
    {
        var topside = TopsideAdapter.Convert(topsideDto);
        var project = _projectService.GetProject(topsideDto.ProjectId);
        topside.Project = project;
        topside.LastChangedDate = DateTimeOffset.Now;
        topside.ProspVersion = topsideDto.ProspVersion;
        var createdTopside = _context.Topsides!.Add(topside);
        await _context.SaveChangesAsync();
        SetCaseLink(topside, sourceCaseId, project);
        return createdTopside.Entity;
    }

    private void SetCaseLink(Topside topside, Guid sourceCaseId, Project project)
    {
        var caseItem = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }

        caseItem.TopsideLink = topside.Id;
        _context.SaveChanges();
    }

    public async Task<ProjectDto> DeleteTopside(Guid topsideId)
    {
        var topside = await GetTopside(topsideId);
        _context.Topsides!.Remove(topside);
        DeleteCaseLinks(topsideId);
        await _context.SaveChangesAsync();
        return _projectService.GetProjectDto(topside.ProjectId);
    }

    private void DeleteCaseLinks(Guid topsideId)
    {
        foreach (var c in _context.Cases!)
        {
            if (c.TopsideLink == topsideId)
            {
                c.TopsideLink = Guid.Empty;
            }
        }
    }

    public ProjectDto UpdateTopside(TopsideDto updatedTopsideDto)
    {
        var existing = GetTopside(updatedTopsideDto.Id).GetAwaiter().GetResult();
        TopsideAdapter.ConvertExisting(existing, updatedTopsideDto);

        if (updatedTopsideDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.TopsideCostProfiles!.Remove(existing.CostProfile);
        }

        if (updatedTopsideDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.TopsideCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        _context.Topsides!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(updatedTopsideDto.ProjectId);
    }

    public TopsideDto NewUpdateTopside(TopsideDto updatedTopsideDto)
    {
        var existing = GetTopside(updatedTopsideDto.Id).GetAwaiter().GetResult();
        TopsideAdapter.ConvertExisting(existing, updatedTopsideDto);

        if (updatedTopsideDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.TopsideCostProfiles!.Remove(existing.CostProfile);
        }

        if (updatedTopsideDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.TopsideCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        var updatedTopside = _context.Topsides!.Update(existing);
        _context.SaveChanges();
        return TopsideDtoAdapter.Convert(updatedTopside.Entity);
    }

    public async Task<Topside> GetTopside(Guid topsideId)
    {
        var topside = await _context.Topsides!
            .Include(c => c.Project)
            .Include(c => c.CostProfile)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == topsideId);
        if (topside == null)
        {
            throw new ArgumentException($"Topside {topsideId} not found.");
        }

        return topside;
    }
}
