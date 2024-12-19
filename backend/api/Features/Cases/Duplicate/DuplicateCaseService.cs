using api.Context;
using api.Context.Extensions;
using api.Features.Images.Copy;
using api.Features.Images.Shared;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseService(DuplicateCaseRepository duplicateCaseRepository, DcdDbContext context, CopyImageService copyImageService)
{
    public async Task DuplicateCase(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await duplicateCaseRepository.GetCaseAndAssetsNoTracking(projectPk, caseId);

        var existingCaseNames = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Select(x => x.Name)
            .Distinct()
            .ToHashSetAsync();

        var duplicateCaseId = Guid.NewGuid();

        ResetIdPropertiesInCaseGraphService.ResetPrimaryKeysAndForeignKeysInGraph(caseItem, duplicateCaseId);

        var utcNow = DateTimeOffset.UtcNow;
        caseItem.CreateTime = utcNow;
        caseItem.ModifyTime = utcNow;
        caseItem.Name = GetUniqueCopyName(existingCaseNames, caseItem.Name);

        context.Cases.Add(caseItem);

        await CopyCaseImages(projectPk, caseId, duplicateCaseId);

        await context.SaveChangesAsync();
    }

    private static string GetUniqueCopyName(HashSet<string> existingCaseNames, string originalName)
    {
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
        var images = await context.Images
            .Where(x => x.ProjectId == projectPk && x.CaseId == sourceCaseId)
            .ToListAsync();

        foreach (var image in images)
        {
            var newImageId = Guid.NewGuid();

            var sourceUrl = ImageHelper.GetBlobName(sourceCaseId, image.ProjectId, image.Id);
            var destinationUrl = ImageHelper.GetBlobName(destinationCaseId, projectPk, newImageId);

            context.Images.Add(new Image
            {
                Id = newImageId,
                ProjectId = projectPk,
                CaseId = destinationCaseId,
                CreateTime = DateTimeOffset.UtcNow,
                Description = image.Description,
                Url = destinationUrl
            });

            await copyImageService.Copy(sourceUrl, destinationUrl);
        }
    }
}
