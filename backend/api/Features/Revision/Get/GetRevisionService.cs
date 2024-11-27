using api.Context;
using api.Exceptions;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revision.Get;

public class GetRevisionService(DcdDbContext context, IMapper mapper)
{
    public async Task<RevisionWithCasesDto> GetRevision(Guid projectId, Guid revisionId)
    {
        var project = await context.Projects
                          .Include(p => p.Cases)
                          .Include(p => p.Wells)
                          .Include(p => p.ExplorationOperationalWellCosts)
                          .Include(p => p.DevelopmentOperationalWellCosts)
                          .FirstOrDefaultAsync(p => (p.Id == revisionId || p.FusionProjectId == revisionId) && p.IsRevision)
                      ?? throw new NotFoundInDBException($"Project with id {revisionId} not found.");

        project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();

        var projectDto = mapper.Map<Project, RevisionWithCasesDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        projectDto.ModifyTime = GetLatestModifyTime(project);

        return projectDto;
    }

    private static DateTimeOffset GetLatestModifyTime(Project project)
    {
        return project.Cases
            .Select(c => c.ModifyTime)
            .Append(project.ModifyTime)
            .Max();
    }
}
