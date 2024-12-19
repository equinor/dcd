using api.Context;
using api.Context.Extensions;
using api.Features.Images.Copy;
using api.Features.Images.Shared;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionService(CreateRevisionRepository createRevisionRepository, DcdDbContext context, CopyImageService copyImageService)
{
    public async Task<Guid> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseIdMapping = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .ToDictionaryAsync(x => x.Id, _ => Guid.NewGuid());

        var revision = await createRevisionRepository.GetProjectAndAssetsNoTracking(projectPk);

        revision.IsRevision = true;
        revision.OriginalProjectId = projectPk;
        revision.InternalProjectPhase = createRevisionDto.InternalProjectPhase ?? revision.InternalProjectPhase;
        revision.Classification = createRevisionDto.Classification ?? revision.Classification;

        ResetIdPropertiesInProjectGraphService.ResetPrimaryKeysAndForeignKeysInGraph(revision, caseIdMapping);

        revision.RevisionDetails = new RevisionDetails
        {
            RevisionName = createRevisionDto.Name,
            Mdqc = createRevisionDto.Mdqc,
            Arena = createRevisionDto.Arena,
            RevisionDate = DateTimeOffset.UtcNow,
            Revision = revision,
            Classification = createRevisionDto.Classification ?? revision.Classification
        };

        context.Projects.Add(revision);

        var existingProject = await context.Projects.SingleAsync(p => p.Id == projectPk);
        existingProject.InternalProjectPhase = createRevisionDto.InternalProjectPhase ?? existingProject.InternalProjectPhase;
        existingProject.Classification = createRevisionDto.Classification ?? existingProject.Classification;

        await CopyProjectImages(projectPk, revision.Id);
        await CopyCaseImages(projectPk, revision.Id, caseIdMapping);

        await context.SaveChangesAsync();

        return revision.Id;
    }

    private async Task CopyProjectImages(Guid projectPk, Guid revisionId)
    {
        var images = await context.Images
            .Where(x => x.ProjectId == projectPk && x.CaseId == null)
            .ToListAsync();

        foreach (var image in images)
        {
            var newImageId = Guid.NewGuid();

            var sourceUrl = ImageHelper.GetBlobName(null, image.ProjectId, image.Id);
            var destinationUrl = ImageHelper.GetBlobName(null, revisionId, newImageId);

            context.Images.Add(new Image
            {
                Id = newImageId,
                ProjectId = revisionId,
                CaseId = null,
                CreateTime = DateTimeOffset.UtcNow,
                Description = image.Description,
                Url = destinationUrl
            });

            await copyImageService.Copy(sourceUrl, destinationUrl);
        }
    }

    private async Task CopyCaseImages(Guid projectPk, Guid revisionId, Dictionary<Guid, Guid> caseIdMapping)
    {
        var images = await context.Images
            .Where(x => x.ProjectId == projectPk && x.CaseId != null)
            .ToListAsync();

        foreach (var image in images)
        {
            var newImageId = Guid.NewGuid();

            var sourceUrl = ImageHelper.GetBlobName(image.CaseId, image.ProjectId, image.Id);
            var destinationUrl = ImageHelper.GetBlobName(caseIdMapping[image.CaseId!.Value], revisionId, newImageId);

            context.Images.Add(new Image
            {
                Id = newImageId,
                ProjectId = revisionId,
                CaseId = caseIdMapping[image.CaseId!.Value],
                CreateTime = DateTimeOffset.UtcNow,
                Description = image.Description,
                Url = destinationUrl
            });

            await copyImageService.Copy(sourceUrl, destinationUrl);
        }
    }
}
