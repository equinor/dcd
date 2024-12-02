using api.Context;
using api.Models;
using api.Services;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Projects.GetWithCases;

public class GetProjectWithCasesService(DcdDbContext context, IMapperService mapperService)
{
    public async Task<ProjectWithCasesDto> GetProjectWithCases(Guid projectId)
    {
        var existingProject = await context.Projects
            .Include(x => x.Cases)
            .SingleAsync(p => p.Id == projectId);

        return mapperService.MapToDto<Project, ProjectWithCasesDto>(existingProject, projectId);
    }
}
