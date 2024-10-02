using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseService : ICaseService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<CaseService> _logger;
    private readonly IMapper _mapper;
    private readonly IMapperService _mapperService;
    private readonly ICaseRepository _repository;

    public CaseService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        ICaseRepository repository,
        IMapperService mapperService,
        IMapper mapper,
        IProjectAccessService projectAccessService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
        _mapper = mapper;
        _mapperService = mapperService;
        _repository = repository;
        _projectAccessService = projectAccessService;
    }

    public async Task<ProjectWithAssetsDto> DeleteCase(Guid projectId, Guid caseId)
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Case>(projectId, caseId);

        var caseItem = await GetCase(caseId);

        _context.Cases!.Remove(caseItem);

        await _context.SaveChangesAsync();

        return await _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<Case> GetCase(Guid caseId)
    {
        var caseItem = await _context.Cases!
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .FirstOrDefaultAsync(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }

    public async Task<Case> GetCaseWithIncludes(Guid caseId, params Expression<Func<Case, object>>[] includes)
    {
        return await _repository.GetCaseWithIncludes(caseId, includes)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");
    }

    // TODO: Delete this method
    public async Task<IEnumerable<Case>> GetAll()
    {
        return await _context.Cases.ToListAsync();
    }

    public async Task<CaseDto> UpdateCase<TDto>(
        Guid projectId,
        Guid caseId,
        TDto updatedCaseDto
    )
        where TDto : BaseUpdateCaseDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Case>(projectId, caseId);

        var existingCase = await _repository.GetCase(caseId)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");

        _mapperService.MapToEntity(updatedCaseDto, existingCase, caseId);

        existingCase.ModifyTime = DateTimeOffset.UtcNow;

        try
        {
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update case with id {caseId}.", caseId);
            throw;
        }

        await _repository.UpdateModifyTime(caseId);
        var dto = _mapperService.MapToDto<Case, CaseDto>(existingCase, caseId);
        return dto;
    }
}
