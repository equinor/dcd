using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellService : IWellService
{
    private readonly IWellRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<WellService> _logger;
    private readonly IMapperService _mapperService;

    public WellService(
        ILoggerFactory loggerFactory,
        IWellRepository repository,
        IMapperService mapperService,
        IProjectRepository projectRepository
        )
    {
        _logger = loggerFactory.CreateLogger<WellService>();
        _repository = repository;
        _mapperService = mapperService;
        _projectRepository = projectRepository;
    }

    private static bool UpdateChangesWellType(Well well, UpdateWellDto updatedWellDto)
    {
        var isWellProjectWell = Well.IsWellProjectWell(well.WellCategory);
        return isWellProjectWell != Well.IsWellProjectWell(updatedWellDto.WellCategory);
    }

    private static bool InvalidWellCategory(UpdateWellDto updatedWellDto)
    {
        return new[] {
            WellCategory.Oil_Producer,
            WellCategory.Gas_Producer,
            WellCategory.Water_Injector,
            WellCategory.Gas_Injector,
            WellCategory.Exploration_Well,
            WellCategory.Appraisal_Well,
            WellCategory.Sidetrack,
        }.Contains(updatedWellDto.WellCategory);
    }

    public async Task<WellDto> UpdateWell(Guid projectId, Guid wellId, UpdateWellDto updatedWellDto)
    {


        var existingWell = await _repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        if (InvalidWellCategory(updatedWellDto))
        {
            throw new InvalidInputException("Invalid well category", wellId);
        }

        if (UpdateChangesWellType(existingWell, updatedWellDto))
        {
            throw new WellChangeTypeException("Cannot change well type", wellId);
        }

        _mapperService.MapToEntity(updatedWellDto, existingWell, wellId);

        try
        {
            await _projectRepository.UpdateModifyTime(projectId);
            await _repository.SaveChangesAsync(); // TODO: run calculations
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well with id {wellId}", wellId);
            throw;
        }

        var dto = _mapperService.MapToDto<Well, WellDto>(existingWell, wellId);
        return dto;
    }

    public async Task<WellDto> CreateWell(Guid projectId, CreateWellDto createWellDto)
    {
        var project = await _projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found");

        Well well = new()
        {
            Project = project
        };

        var newWell = _mapperService.MapToEntity(createWellDto, well, projectId);

        Well createdWell;
        try
        {
            createdWell = _repository.AddWell(newWell);
            await _projectRepository.UpdateModifyTime(projectId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create well");
            throw;
        }

        var dto = _mapperService.MapToDto<Well, WellDto>(createdWell, createdWell.Id);
        return dto;
    }

    public async Task DeleteWell(Guid projectId, Guid wellId)
    {
        var well = await _repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        _repository.DeleteWell(well);
        await _projectRepository.UpdateModifyTime(projectId);
        await _repository.SaveChangesAsync(); // TODO: Run calculations
    }

    public async Task<IEnumerable<CaseDto>> GetAffectedCases(Guid wellId)
    {
        var _ = await _repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        var cases = await _repository.GetCasesAffectedByDeleteWell(wellId);

        var dtos = new List<CaseDto>();
        foreach (var c in cases)
        {
            var dto = _mapperService.MapToDto<Case, CaseDto>(c, c.Id);
            dtos.Add(dto);
        }
        return dtos;
    }
}
