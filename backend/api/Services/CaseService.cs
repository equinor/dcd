using System.Collections.Immutable;
using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ILogger<CaseService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _context = context;
        _projectService = projectService;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto CreateCase(CaseDto caseDto)
    {
        var case_ = CaseAdapter.Convert(caseDto);
        case_.CreateTime = DateTime.UtcNow;
        if (case_.DG4Date == DateTimeOffset.MinValue)
        {
            case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }
        var project = _projectService.GetProject(case_.ProjectId);
        case_.Project = project;
        _context.Cases!.Add(case_);
        _context.SaveChanges();
        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto DuplicateCase(Guid caseId)
    {
        var case_ = GetCase(caseId);
        case_.Id = new Guid();
        if (case_.DG4Date == DateTimeOffset.MinValue)
        {
            case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }
        var project = _projectService.GetProject(case_.ProjectId);
        case_.Project = project;

        List<Case> duplicateCaseNames = new List<Case>();
        foreach (Case c in project.Cases!)
        {
            string copyNumber = c.Name.Substring(c.Name.Length - 1, 1);
            if (c.Name.Equals(case_.Name) || c.Name.Equals(case_.Name + " - copy #" + copyNumber))
            {
                duplicateCaseNames.Add(c);
            }
        }
        case_.Name = case_.Name + " - copy #" + duplicateCaseNames.Count();
        _context.Cases!.Add(case_);
        _context.SaveChanges();
        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto UpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = GetCase(updatedCaseDto.Id);
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        _context.SaveChanges();
        return _projectService.GetProjectDto(caseItem.ProjectId);
    }

        public CaseDto NewUpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = GetCase(updatedCaseDto.Id);
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        _context.SaveChanges();
        return CaseDtoAdapter.Convert(GetCase(caseItem.Id));
    }

    public ProjectDto DeleteCase(Guid caseId)
    {
        var caseItem = GetCase(caseId);
        _context.Cases!.Remove(caseItem);
        _context.SaveChanges();
        return _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public Case GetCase(Guid caseId)
    {
        var caseItem = _context.Cases!
            .FirstOrDefault(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }
}
