using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationService : IExplorationService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;

    private readonly ILogger<ExplorationService> _logger;
    private readonly IMapper _mapper;

    public ExplorationService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
        _mapper = mapper;
    }


    public async Task<ExplorationDto> CopyExploration(Guid explorationId, Guid sourceCaseId)
    {
        var source = await GetExploration(explorationId);
        var newExplorationDto = _mapper.Map<ExplorationDto>(source);
        if (newExplorationDto == null)
        {
            throw new ArgumentNullException(nameof(newExplorationDto));
        }
        newExplorationDto.Id = Guid.Empty;

        if (newExplorationDto.ExplorationWellCostProfile != null)
        {
            newExplorationDto.ExplorationWellCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.AppraisalWellCostProfile != null)
        {
            newExplorationDto.AppraisalWellCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.SidetrackCostProfile != null)
        {
            newExplorationDto.SidetrackCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.SeismicAcquisitionAndProcessing != null)
        {
            newExplorationDto.SeismicAcquisitionAndProcessing.Id = Guid.Empty;
        }
        if (newExplorationDto.CountryOfficeCost != null)
        {
            newExplorationDto.CountryOfficeCost.Id = Guid.Empty;
        }
        if (newExplorationDto.GAndGAdminCost != null)
        {
            newExplorationDto.GAndGAdminCost.Id = Guid.Empty;
        }

        // var wellProject = await NewCreateExploration(newExplorationDto, sourceCaseId);
        // var dto = ExplorationDtoAdapter.Convert(wellProject);
        // return dto;
        return newExplorationDto;
    }

    public async Task<Exploration> NewCreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto)
    {
        var exploration = _mapper.Map<Exploration>(explorationDto);
        if (exploration == null)
        {
            throw new ArgumentNullException(nameof(exploration));
        }
        var project = await _projectService.GetProject(projectId);
        exploration.Project = project;
        var createdExploration = _context.Explorations!.Add(exploration);
        await _context.SaveChangesAsync();
        await SetCaseLink(exploration, sourceCaseId, project);
        return createdExploration.Entity;
    }

    private async Task SetCaseLink(Exploration exploration, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.ExplorationLink = exploration.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<ExplorationDto> NewUpdateExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = await GetExploration(updatedExplorationDto.Id);
        _mapper.Map(updatedExplorationDto, existing);

        var updatedExploration = _context.Explorations!.Update(existing);
        await _context.SaveChangesAsync();
        var explorationDto = _mapper.Map<ExplorationDto>(updatedExploration.Entity);
        if (explorationDto == null)
        {
            throw new ArgumentNullException(nameof(explorationDto));
        }
        return explorationDto;
    }

    public async Task<ExplorationDto[]> UpdateMultiple(ExplorationDto[] updatedExplorationDtos)
    {
        var updatedExplorationDtoList = new List<ExplorationDto>();
        foreach (var explorationDto in updatedExplorationDtos)
        {
            var updatedExplorationDto = await UpdateSingleExploration(explorationDto);
            updatedExplorationDtoList.Add(updatedExplorationDto);
        }

        await _context.SaveChangesAsync();
        return updatedExplorationDtoList.ToArray();
    }

    public async Task<ExplorationDto> UpdateSingleExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = await GetExploration(updatedExplorationDto.Id);

        _mapper.Map(updatedExplorationDto, existing);

        var exploration = _context.Explorations!.Update(existing);
        await _context.SaveChangesAsync();
        var explorationDto = _mapper.Map<ExplorationDto>(exploration.Entity);
        if (explorationDto == null)
        {
            throw new ArgumentNullException(nameof(explorationDto));
        }
        return explorationDto;
    }

    public async Task<Exploration> GetExploration(Guid explorationId)
    {
        var exploration = await _context.Explorations!
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .FirstOrDefaultAsync(o => o.Id == explorationId);
        if (exploration == null)
        {
            throw new ArgumentException(string.Format("Exploration {0} not found.", explorationId));
        }
        return exploration;
    }
}
