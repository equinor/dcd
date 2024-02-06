
using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureService : ISubstructureService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SubstructureService> _logger;

    public SubstructureService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SubstructureService>();
    }

    public async Task<ProjectDto> CreateSubstructure(Substructure substructure, Guid sourceCaseId)
    {
        var project = await _projectService.GetProject(substructure.ProjectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Substructures!.Add(substructure);
        await _context.SaveChangesAsync();
        await SetCaseLink(substructure, sourceCaseId, project);
        return await _projectService.GetProjectDto(project.Id);
    }

    public async Task<Substructure> NewCreateSubstructure(SubstructureDto substructureDto, Guid sourceCaseId)
    {
        var substructure = SubstructureAdapter.Convert(substructureDto);
        var project = await _projectService.GetProject(substructure.ProjectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.UtcNow;
        var createdSubstructure = _context.Substructures!.Add(substructure);
        await _context.SaveChangesAsync();
        await SetCaseLink(substructure, sourceCaseId, project);
        return createdSubstructure.Entity;
    }

    public async Task<SubstructureDto> CopySubstructure(Guid substructureId, Guid sourceCaseId)
    {
        var source = await GetSubstructure(substructureId);
        var newSubstructureDto = SubstructureDtoAdapter.Convert(source);
        newSubstructureDto.Id = Guid.Empty;
        if (newSubstructureDto.CostProfile != null)
        {
            newSubstructureDto.CostProfile.Id = Guid.Empty;
        }
        if (newSubstructureDto.CostProfileOverride != null)
        {
            newSubstructureDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newSubstructureDto.CessationCostProfile != null)
        {
            newSubstructureDto.CessationCostProfile.Id = Guid.Empty;
        }

        var topside = await NewCreateSubstructure(newSubstructureDto, sourceCaseId);
        var dto = SubstructureDtoAdapter.Convert(topside);

        return dto;
    }

    private async Task SetCaseLink(Substructure substructure, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.SubstructureLink = substructure.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectDto> UpdateSubstructure(SubstructureDto updatedSubstructureDto)
    {
        var existing = await GetSubstructure(updatedSubstructureDto.Id);

        SubstructureAdapter.ConvertExisting(existing, updatedSubstructureDto);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Substructures!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(existing.ProjectId);
    }

    public async Task<Substructure> GetSubstructure(Guid substructureId)
    {
        var substructure = await _context.Substructures!
            .Include(c => c.Project)
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == substructureId);
        if (substructure == null)
        {
            throw new ArgumentException(string.Format("Substructure {0} not found.", substructureId));
        }
        return substructure;
    }
}
