using api.Context;
using api.Context.Extensions;
using api.Features.Images.Copy;
using api.Features.Images.Shared;
using api.Features.Revisions.Create;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseService(DuplicateCaseRepository duplicateCaseRepository, DcdDbContext context, CopyImageService copyImageService)
{
    public async Task DuplicateCase(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingCase = await duplicateCaseRepository.GetFullCaseGraph(projectPk, caseId);

        var duplicate = ProjectDuplicator.DuplicateCase(existingCase, projectPk, Guid.NewGuid());

        duplicate.Name = await GetUniqueCopyName(projectPk, existingCase.Name);

        context.Cases.Add(duplicate);

        await CopyCaseImages(projectPk, caseId, duplicate.Id);

        await context.SaveChangesAsync();
    }

    private async Task<string> GetUniqueCopyName(Guid projectPk, string originalName)
    {
        var existingCaseNames = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Select(x => x.Name)
            .Distinct()
            .ToHashSetAsync();

        var firstCopyName = originalName + " - copy";

        if (!existingCaseNames.Contains(firstCopyName))
        {
            return firstCopyName;
        }

        var i = 1;

        while (true)
        {
            var nameSuggestion = $"{originalName} - copy ({++i})";

            if (!existingCaseNames.Contains(nameSuggestion))
            {
                return nameSuggestion;
            }
        }
    }

    private async Task CopyCaseImages(Guid projectPk, Guid sourceCaseId, Guid destinationCaseId)
    {
        var images = await context.CaseImages
            .Where(x => x.Case.ProjectId == projectPk && x.CaseId == sourceCaseId)
            .ToListAsync();

        foreach (var image in images)
        {
            var newImageId = Guid.NewGuid();

            var sourceUrl = ImageHelper.GetCaseBlobName(sourceCaseId, image.Id);
            var destinationUrl = ImageHelper.GetCaseBlobName(destinationCaseId, newImageId);

            context.CaseImages.Add(new CaseImage
            {
                Id = newImageId,
                CaseId = destinationCaseId,
                Description = image.Description,
                Url = destinationUrl
            });

            await copyImageService.Copy(sourceUrl, destinationUrl);
        }
    }
}
