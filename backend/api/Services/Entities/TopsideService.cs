using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TopsideService : ITopsideService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<TopsideService> _logger;
    private readonly IMapper _mapper;
    private readonly ITopsideRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public TopsideService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ITopsideRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TopsideService>();
        _mapper = mapper;
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<TopsideWithProfilesDto> CopyTopside(Guid topsideId, Guid sourceCaseId)
    {
        var source = await GetTopside(topsideId);
        var newTopsideDto = _mapper.Map<TopsideWithProfilesDto>(source);
        if (newTopsideDto == null)
        {
            _logger.LogError("Failed to map topside to dto");
            throw new Exception("Failed to map topside to dto");
        }
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

        // var topside = await NewCreateTopside(newTopsideDto, sourceCaseId);
        // var dto = TopsideDtoAdapter.Convert(topside);

        // return dto;
        return newTopsideDto;
    }

    public async Task<Topside> CreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto)
    {
        var topside = _mapper.Map<Topside>(topsideDto);
        if (topside == null)
        {
            throw new ArgumentNullException(nameof(topside));
        }
        var project = await _projectService.GetProject(projectId);
        topside.Project = project;
        topside.LastChangedDate = DateTimeOffset.UtcNow;
        var createdTopside = _context.Topsides!.Add(topside);
        SetCaseLink(topside, sourceCaseId, project);
        await _context.SaveChangesAsync();

        return createdTopside.Entity;
    }

    private static void SetCaseLink(Topside topside, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.TopsideLink = topside.Id;
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

    public async Task<TopsideDto> UpdateTopside<TDto>(
        Guid caseId,
        Guid topsideId,
        TDto updatedTopsideDto
    )
        where TDto : BaseUpdateTopsideDto
    {
        var existingTopside = await _repository.GetTopside(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        _mapperService.MapToEntity(updatedTopsideDto, existingTopside, topsideId);
        existingTopside.LastChangedDate = DateTimeOffset.UtcNow;

        Topside updatedTopside;
        try
        {
            updatedTopside = _repository.UpdateTopside(existingTopside);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update topside with id {topsideId} for case id {caseId}.", topsideId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Topside, TopsideDto>(updatedTopside, topsideId);
        return dto;
    }

    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        Guid caseId,
        Guid topsideId,
        Guid costProfileId,
        UpdateTopsideCostProfileOverrideDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfileOverride, TopsideCostProfileOverrideDto, UpdateTopsideCostProfileOverrideDto>(
            caseId,
            topsideId,
            costProfileId,
            dto,
            _repository.GetTopsideCostProfileOverride,
            _repository.UpdateTopsideCostProfileOverride
        );
    }

    public async Task<TopsideCostProfileDto> AddOrUpdateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto
    )
    {
        var topside = await _repository.GetTopsideWithCostProfile(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        if (topside.CostProfile != null)
        {
            return await UpdateTopsideCostProfile(caseId, topsideId, topside.CostProfile.Id, dto);
        }

        return await CreateTopsideCostProfile(caseId, topsideId, dto, topside);
    }

    private async Task<TopsideCostProfileDto> UpdateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        UpdateTopsideCostProfileDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfile, TopsideCostProfileDto, UpdateTopsideCostProfileDto>(
            caseId,
            topsideId,
            profileId,
            dto,
            _repository.GetTopsideCostProfile,
            _repository.UpdateTopsideCostProfile
        );
    }

    private async Task<TopsideCostProfileDto> CreateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto,
        Topside topside
    )
    {
        TopsideCostProfile topsideCostProfile = new()
        {
            Topside = topside
        };

        var newProfile = _mapperService.MapToEntity(dto, topsideCostProfile, topsideId);

        try
        {
            _repository.CreateTopsideCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for topside with id {topsideId} for case id {caseId}.", topsideId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<TopsideCostProfile, TopsideCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    private async Task<TDto> UpdateTopsideTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid topsideId,
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

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, topsideId);

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
