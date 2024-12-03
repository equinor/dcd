using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Features.Revisions.Get;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Projects.GetWithAssets;

public class GetProjectWithAssetsService(DcdDbContext context, IProjectWithAssetsRepository projectWithAssetsRepository, IMapper mapper)
{
    public async Task<ProjectWithAssetsDto> GetProjectWithAssets(Guid projectId)
    {
        var project = await projectWithAssetsRepository.GetProjectWithCasesAndAssets(projectId);
        var revisionDetails = await context.RevisionDetails
            .Where(r => r.OriginalProjectId == project.Id)
            .OrderBy(x => x.RevisionDate)
            .Select(x => new RevisionDetailsDto
            {
                Id = x.Id,
                OriginalProjectId = x.OriginalProjectId,
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.RevisionDate,
                Arena = x.Arena,
                Mdqc = x.Mdqc,
                Classification = x.Classification
            })
            .ToListAsync();

        var projectDto = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        projectDto.RevisionsDetailsList = revisionDetails;
        projectDto.ModifyTime = project.Cases.Select(c => c.ModifyTime).Append(project.ModifyTime).Max();

        return projectDto;
    }
}
