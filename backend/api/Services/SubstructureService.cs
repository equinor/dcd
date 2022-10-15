
using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ILogger<SubstructureService> _logger;

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
        else
        {
            return new List<Substructure>();
        }
    }

    public ProjectDto CreateSubstructure(Substructure substructure, Guid sourceCaseId)
    {
        var project = _projectService.GetProject(substructure.ProjectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.Now;
        _context.Substructures!.Add(substructure);
        _context.SaveChanges();
        SetCaseLink(substructure, sourceCaseId, project);
        return _projectService.GetProjectDto(project.Id);
    }

    public Substructure NewCreateSubstructure(SubstructureDto substructureDto, Guid sourceCaseId)
    {
        var substructure = SubstructureAdapter.Convert(substructureDto);
        var project = _projectService.GetProject(substructure.ProjectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.Now;
        var createdSubstructure = _context.Substructures!.Add(substructure);
        _context.SaveChanges();
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

    public ProjectDto DeleteSubstructure(Guid substructureId)
    {
        var substructure = GetSubstructure(substructureId);
        _context.Substructures!.Remove(substructure);
        DeleteCaseLinks(substructureId);
        _context.SaveChanges();
        return _projectService.GetProjectDto(substructure.ProjectId);
    }

    private void DeleteCaseLinks(Guid substructureId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.SubstructureLink == substructureId)
            {
                c.SubstructureLink = Guid.Empty;
            }
        }
    }

    public ProjectDto UpdateSubstructure(SubstructureDto updatedSubstructureDto)
    {
        var existing = GetSubstructure(updatedSubstructureDto.Id);

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
        var existing = GetSubstructure(updatedSubstructureDto.Id);

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
        var createdSubstructure = _context.Substructures!.Update(existing);
        _context.SaveChanges();
        return SubstructureDtoAdapter.Convert(createdSubstructure.Entity);
    }


    public Substructure GetSubstructure(Guid substructureId)
    {
        var substructure = _context.Substructures!
            .Include(c => c.Project)
            .Include(c => c.CostProfile)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefault(o => o.Id == substructureId);
        if (substructure == null)
        {
            throw new ArgumentException(string.Format("Substructure {0} not found.", substructureId));
        }
        return substructure;
    }
}
