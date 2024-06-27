using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfService : ISurfService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    private readonly IMapper _mapper;
    private readonly ISurfRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;
    public SurfService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ISurfRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
        _mapper = mapper;
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
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

    public async Task<Surf> CreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto)
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
        SetCaseLink(surf, sourceCaseId, project);
        await _context.SaveChangesAsync();
        return createdSurf.Entity;
    }

    private static void SetCaseLink(Surf surf, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.SurfLink = surf.Id;
    }

    public async Task<SurfDto> UpdateSurf<TDto>(
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    )
        where TDto : BaseUpdateSurfDto
    {
        var existingSurf = await _repository.GetSurf(surfId)
            ?? throw new ArgumentException(string.Format($"Surf with id {surfId} not found."));

        _mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTimeOffset.UtcNow;

        // Surf updatedSurf;
        try
        {
            // updatedSurf = _repository.UpdateSurf(existingSurf);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }


        var dto = _mapperService.MapToDto<Surf, SurfDto>(existingSurf, surfId);
        return dto;
    }
}
