using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseService(DuplicateCaseRepository duplicateCaseRepository, DcdDbContext context)
{
    public async Task DuplicateCase(Guid caseId)
    {
        var caseItem = await duplicateCaseRepository.GetCaseAndAssetsNoTracking(caseId);

        var existingCaseNames = await context.Cases
            .Where(x => x.ProjectId == caseItem.ProjectId)
            .Select(x => x.Name)
            .Distinct()
            .ToHashSetAsync();

        ResetIdPropertiesInCaseGraphService.ResetPrimaryKeysAndForeignKeysInGraph(caseItem);

        var utcNow = DateTimeOffset.UtcNow;
        caseItem.CreateTime = utcNow;
        caseItem.ModifyTime = utcNow;
        caseItem.Name = GetUniqueCopyName(existingCaseNames, caseItem.Name);

        context.Cases.Add(caseItem);

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
}
