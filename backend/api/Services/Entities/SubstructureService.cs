
using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureService : ISubstructureService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SubstructureService> _logger;
    private readonly IMapper _mapper;
    private readonly ISubstructureRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public SubstructureService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ISubstructureRepository substructureRepository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SubstructureService>();
        _mapper = mapper;
        _repository = substructureRepository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<Substructure> CreateSubstructure(Guid projectId, Guid sourceCaseId, CreateSubstructureDto substructureDto)
    {
        var substructure = _mapper.Map<Substructure>(substructureDto);
        if (substructure == null)
        {
            throw new ArgumentNullException(nameof(substructure));
        }
        var project = await _projectService.GetProject(projectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.UtcNow;
        var createdSubstructure = _context.Substructures!.Add(substructure);
        SetCaseLink(substructure, sourceCaseId, project);
        await _context.SaveChangesAsync();
        return createdSubstructure.Entity;
    }

    private static void SetCaseLink(Substructure substructure, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.SubstructureLink = substructure.Id;
    }

    public async Task<SubstructureWithProfilesDto> CopySubstructure(Guid substructureId, Guid sourceCaseId)
    {
        var source = await GetSubstructure(substructureId);
        var newSubstructureDto = _mapper.Map<SubstructureWithProfilesDto>(source);
        if (newSubstructureDto == null)
        {
            throw new ArgumentNullException(nameof(newSubstructureDto));
        }
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

        // var topside = await NewCreateSubstructure(newSubstructureDto, sourceCaseId);
        // var dto = SubstructureDtoAdapter.Convert(topside);

        // return dto;
        return newSubstructureDto;
    }

    public async Task<Substructure> GetSubstructure(Guid substructureId)
    {
        var substructure = await _context.Substructures!
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

    public async Task<SubstructureDto> UpdateSubstructure<TDto>(
        Guid caseId,
        Guid substructureId,
        TDto updatedSubstructureDto
    )
        where TDto : BaseUpdateSubstructureDto
    {
        var existingSubstructure = await _repository.GetSubstructure(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        _mapperService.MapToEntity(updatedSubstructureDto, existingSubstructure, substructureId);
        existingSubstructure.LastChangedDate = DateTimeOffset.UtcNow;

        Substructure updatedSubstructure;
        try
        {
            updatedSubstructure = _repository.UpdateSubstructure(existingSubstructure);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update substructure with id {SubstructureId} for case id {CaseId}.", substructureId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Substructure, SubstructureDto>(updatedSubstructure, substructureId);

        return dto;
    }

    public async Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        var substructure = await _repository.GetSubstructureWithCostProfile(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        if (substructure.CostProfile != null)
        {
            return await UpdateSubstructureCostProfile(caseId, substructureId, substructure.CostProfile.Id, dto);
        }

        return await CreateSubstructureCostProfile(caseId, substructureId, dto, substructure);
    }

    private async Task<SubstructureCostProfileDto> UpdateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfile, SubstructureCostProfileDto, UpdateSubstructureCostProfileDto>(
            caseId,
            substructureId,
            profileId,
            dto,
            _repository.GetSubstructureCostProfile,
            _repository.UpdateSubstructureCostProfile
        );
    }

    private async Task<SubstructureCostProfileDto> CreateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto,
        Substructure substructure
    )
    {
        SubstructureCostProfile substructureCostProfile = new SubstructureCostProfile
        {
            Substructure = substructure
        };

        var newProfile = _mapperService.MapToEntity(dto, substructureCostProfile, substructureId);

        try
        {
            _repository.CreateSubstructureCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for substructure with id {substructureId} for case id {caseId}.", substructureId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        Guid caseId,
        Guid substructureId,
        Guid costProfileId,
        UpdateSubstructureCostProfileOverrideDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto, UpdateSubstructureCostProfileOverrideDto>(
        caseId,
        substructureId,
        costProfileId,
        dto,
        _repository.GetSubstructureCostProfileOverride,
        _repository.UpdateSubstructureCostProfileOverride
);
    }

    private async Task<TDto> UpdateSubstructureTimeSeries<TProfile, TDto, TUpdateDto>(
    Guid caseId,
    Guid substructureId,
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

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, substructureId);

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
