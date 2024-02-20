using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfService : ISurfService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    private readonly IMapper _mapper;
    public SurfService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
        _mapper = mapper;
    }

    public async Task<SurfDto> CopySurf(Guid surfId, Guid sourceCaseId)
    {
        var source = await GetSurf(surfId);
        var newSurfDto = _mapper.Map<SurfDto>(source);
        if (newSurfDto == null)
        {
            _logger.LogError("Failed to map surf to dto");
            throw new Exception("Failed to map surf to dto");
        }
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

        // var surf = await NewCreateSurf(newSurfDto, sourceCaseId);
        // var dto = SurfDtoAdapter.Convert(surf);

        // return dto;
        return newSurfDto;
    }

    public async Task<ProjectDto> UpdateSurf(SurfDto updatedSurfDto)
    {
        var existing = await GetSurf(updatedSurfDto.Id);
        _mapper.Map(updatedSurfDto, existing);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(existing.ProjectId);
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
        var surf = _mapper.Map<Surf>(surfDto);
        if (surf == null)
        {
            throw new ArgumentNullException(nameof(surf));
        }
        var project = await _projectService.GetProject(surf.ProjectId);
        surf.Project = project;
        surf.ProspVersion = surfDto.ProspVersion;
        surf.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Add(surf);
        await _context.SaveChangesAsync();
        await SetCaseLink(surf, sourceCaseId, project);
        return await _projectService.GetProjectDto(surf.ProjectId);
    }

    public async Task<Surf> NewCreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto)
    {
        var surf = _mapper.Map<Surf>(surfDto);
        if (surf == null)
        {
            throw new ArgumentNullException(nameof(surf));
        }
        var project = await _projectService.GetProject(projectId);
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
}
