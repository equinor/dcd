using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellService(
    ILoggerFactory loggerFactory,
    IWellRepository repository,
    IMapperService mapperService,
    IProjectRepository projectRepository)
    : IWellService
{
    private readonly ILogger<WellService> _logger = loggerFactory.CreateLogger<WellService>();

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


        var existingWell = await repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        if (InvalidWellCategory(updatedWellDto))
        {
            throw new InvalidInputException("Invalid well category", wellId);
        }

        if (UpdateChangesWellType(existingWell, updatedWellDto))
        {
            throw new WellChangeTypeException("Cannot change well type", wellId);
        }

        mapperService.MapToEntity(updatedWellDto, existingWell, wellId);

        try
        {
            await projectRepository.UpdateModifyTime(projectId);
            await repository.SaveChangesAsync(); // TODO: run calculations
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well with id {wellId}", wellId);
            throw;
        }

        var dto = mapperService.MapToDto<Well, WellDto>(existingWell, wellId);
        return dto;
    }

    public async Task<WellDto> CreateWell(Guid projectId, CreateWellDto createWellDto)
    {
        var project = await projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found");

        Well well = new()
        {
            Project = project
        };

        var newWell = mapperService.MapToEntity(createWellDto, well, projectId);

        Well createdWell;
        try
        {
            createdWell = repository.AddWell(newWell);
            await projectRepository.UpdateModifyTime(projectId);
            await repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create well");
            throw;
        }

        var dto = mapperService.MapToDto<Well, WellDto>(createdWell, createdWell.Id);
        return dto;
    }

    public async Task DeleteWell(Guid projectId, Guid wellId)
    {
        var well = await repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        repository.DeleteWell(well);
        await projectRepository.UpdateModifyTime(projectId);
        await repository.SaveChangesAsync(); // TODO: Run calculations
    }

    public async Task<IEnumerable<CaseDto>> GetAffectedCases(Guid wellId)
    {
        _ = await repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        var cases = await repository.GetCasesAffectedByDeleteWell(wellId);

        var dtos = new List<CaseDto>();
        foreach (var c in cases)
        {
            var dto = mapperService.MapToDto<Case, CaseDto>(c, c.Id);
            dtos.Add(dto);
        }
        return dtos;
    }
}
