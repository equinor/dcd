using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Repositories;

public interface IProjectWithAssetsRepository
{
    Task<Project> GetProjectWithCases(Guid projectPk);
}

public class ProjectWithCasesRepository(DcdDbContext context) : IProjectWithAssetsRepository
{
    public async Task<Project> GetProjectWithCases(Guid projectPk)
    {
        return await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);
    }
}
