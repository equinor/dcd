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

    public async Task<SurfWithProfilesDto> CopySurf(Guid surfId, Guid sourceCaseId)
    {
        var source = await GetSurf(surfId);
        var newSurfDto = _mapper.Map<SurfWithProfilesDto>(source);
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

        Surf updatedSurf;
        try
        {
            updatedSurf = _repository.UpdateSurf(existingSurf);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }


        var dto = _mapperService.MapToDto<Surf, SurfDto>(updatedSurf, surfId);
        return dto;
    }

    public async Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    )
    {
        var surf = await _repository.GetSurfWithCostProfile(surfId)
            ?? throw new NotFoundInDBException($"Surf with id {surfId} not found.");

        if (surf.CostProfile != null)
        {
            return await UpdateSurfCostProfile(caseId, surfId, surf.CostProfile.Id, dto);
        }

        return await CreateSurfCostProfile(caseId, surfId, dto, surf);
    }

    private async Task<SurfCostProfileDto> UpdateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        Guid profileId,
        UpdateSurfCostProfileDto dto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfile, SurfCostProfileDto, UpdateSurfCostProfileDto>(
            caseId,
            surfId,
            profileId,
            dto,
            _repository.GetSurfCostProfile,
            _repository.UpdateSurfCostProfile
        );
    }

    private async Task<SurfCostProfileDto> CreateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto,
        Surf surf
    )
    {
        SurfCostProfile surfCostProfile = new SurfCostProfile
        {
            Surf = surf
        };

        var newProfile = _mapperService.MapToEntity(dto, surfCostProfile, surfId);

        try
        {
            _repository.CreateSurfCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfileOverride, SurfCostProfileOverrideDto, UpdateSurfCostProfileOverrideDto>(
            caseId,
            surfId,
            costProfileId,
            updatedSurfCostProfileOverrideDto,
            _repository.GetSurfCostProfileOverride,
            _repository.UpdateSurfCostProfileOverride
        );
    }

    private async Task<TDto> UpdateSurfTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid surfId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, surfId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(updatedProfile, profileId);
        return updatedDto;
    }
}
