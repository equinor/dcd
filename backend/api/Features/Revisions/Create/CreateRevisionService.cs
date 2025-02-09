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

        var project = await createRevisionRepository.GetFullProjectGraph(projectPk);

        var caseIdMapping = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .ToDictionaryAsync(x => x.Id, _ => Guid.NewGuid());

        var revision = ProjectDuplicator.DuplicateProject(project, createRevisionDto, caseIdMapping);

        context.Projects.Add(revision);

        project.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        project.Classification = createRevisionDto.Classification;

        await CopyProjectImages(projectPk, revision.Id);
        await CopyCaseImages(projectPk, caseIdMapping);

        await context.SaveChangesAsync();

        return revision.Id;
    }

    private async Task CopyProjectImages(Guid projectPk, Guid revisionId)
    {
        var images = await context.ProjectImages
            .Where(x => x.ProjectId == projectPk)
            .ToListAsync();

        foreach (var image in images)
        {
            var newImageId = Guid.NewGuid();

            var sourceUrl = ImageHelper.GetProjectBlobName(image.ProjectId, image.Id);
            var destinationUrl = ImageHelper.GetProjectBlobName(revisionId, newImageId);

            context.ProjectImages.Add(new ProjectImage
            {
                Id = newImageId,
                ProjectId = revisionId,
                Description = image.Description,
                Url = destinationUrl
            });

            await copyImageService.Copy(sourceUrl, destinationUrl);
        }
    }

    private async Task CopyCaseImages(Guid projectPk, Dictionary<Guid, Guid> caseIdMapping)
    {
        var images = await context.CaseImages
            .Where(x => x.Case.ProjectId == projectPk)
            .ToListAsync();

        foreach (var image in images)
        {
            var newImageId = Guid.NewGuid();

            var sourceUrl = ImageHelper.GetCaseBlobName(image.CaseId, image.Id);
            var destinationUrl = ImageHelper.GetCaseBlobName(caseIdMapping[image.CaseId], newImageId);

            context.CaseImages.Add(new CaseImage
            {
                Id = newImageId,
                CaseId = caseIdMapping[image.CaseId],
                Description = image.Description,
                Url = destinationUrl
            });

            await copyImageService.Copy(sourceUrl, destinationUrl);
        }
    }
}
