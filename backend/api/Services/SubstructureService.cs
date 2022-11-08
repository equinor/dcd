using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureService
{
    private readonly DcdDbContext _context;
    private readonly ILogger<SubstructureService> _logger;
    private readonly ProjectService _projectService;

    public SubstructureService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SubstructureService>();
    }

    public IEnumerable<Substructure> GetSubstructures(Guid projectId)
    {
        if (_context.Substructures != null)
        {
            return _context.Substructures
                .Include(c => c.CostProfile)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }

        return new List<Substructure>();
    }

    public async Task<ProjectDto> CreateSubstructure(Substructure substructure, Guid sourceCaseId)
    {
        var project = _projectService.GetProject(substructure.ProjectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.Now;
        _context.Substructures!.Add(substructure);
        await _context.SaveChangesAsync();
        SetCaseLink(substructure, sourceCaseId, project);
        return _projectService.GetProjectDto(project.Id);
    }

    public async Task<Substructure> NewCreateSubstructure(SubstructureDto substructureDto, Guid sourceCaseId)
    {
        var substructure = SubstructureAdapter.Convert(substructureDto);
        var project = _projectService.GetProject(substructure.ProjectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.Now;
        var createdSubstructure = _context.Substructures!.Add(substructure);
        await _context.SaveChangesAsync();
        SetCaseLink(substructure, sourceCaseId, project);
        return createdSubstructure.Entity;
    }

    private void SetCaseLink(Substructure substructure, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }

        case_.SubstructureLink = substructure.Id;
        _context.SaveChanges();
    }

    public async Task<ProjectDto> DeleteSubstructure(Guid substructureId)
    {
        var substructure = await GetSubstructure(substructureId);
        _context.Substructures!.Remove(substructure);
        DeleteCaseLinks(substructureId);
        await _context.SaveChangesAsync();
        return _projectService.GetProjectDto(substructure.ProjectId);
    }

    private void DeleteCaseLinks(Guid substructureId)
    {
        foreach (var c in _context.Cases!)
        {
            if (c.SubstructureLink == substructureId)
            {
                c.SubstructureLink = Guid.Empty;
            }
        }
    }

    public ProjectDto UpdateSubstructure(SubstructureDto updatedSubstructureDto)
    {
        var existing = GetSubstructure(updatedSubstructureDto.Id).Result;

        SubstructureAdapter.ConvertExisting(existing, updatedSubstructureDto);

        if (updatedSubstructureDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.SubstructureCostProfiles!.Remove(existing.CostProfile);
        }

        if (updatedSubstructureDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.SubstructureCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        _context.Substructures!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(existing.ProjectId);
    }

    public SubstructureDto NewUpdateSubstructure(SubstructureDto updatedSubstructureDto)
    {
        var existing = GetSubstructure(updatedSubstructureDto.Id).Result;

        SubstructureAdapter.ConvertExisting(existing, updatedSubstructureDto);

        if (updatedSubstructureDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.SubstructureCostProfiles!.Remove(existing.CostProfile);
        }

        if (updatedSubstructureDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.SubstructureCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        var updatedSubstructure = _context.Substructures!.Update(existing);
        _context.SaveChanges();
        return SubstructureDtoAdapter.Convert(updatedSubstructure.Entity);
    }

    public async Task<Substructure> GetSubstructure(Guid substructureId)
    {
        var substructure = await _context.Substructures!
            .Include(c => c.Project)
            .Include(c => c.CostProfile)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == substructureId);
        if (substructure == null)
        {
            throw new ArgumentException($"Substructure {substructureId} not found.");
        }

        return substructure;
    }
}
