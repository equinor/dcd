using api.Context;
using api.Exceptions;
using api.Features.ProjectMembers.Get.Sync.Models;
using api.Models;
using api.Models.Enums;

using Fusion.Integration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Abstractions;

namespace api.Features.ProjectMembers.Get.Sync;

public class FusionOrgChartProjectMemberService(
    DcdDbContext context,
    IDownstreamApi downstreamApi,
    IFusionContextResolver fusionContextResolver,
    ILogger<FusionOrgChartProjectMemberService> logger)
{
    public async Task SyncPmtMembersOnProject(Guid projectId, Guid fusionContextId)
    {
        await VerifyProjectMasterId(projectId, fusionContextId);

        var pmtProjectMembers = await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.FromOrgChart)
            .ToListAsync();

        var dbAzureUserIds = pmtProjectMembers.Select(x => x.AzureAdUserId).ToList();

        var fusionUsers = await GetAllPersonsOnProject(fusionContextId);

        var fusionAzureUserIds = fusionUsers.Select(x => x.AzureUniqueId).ToList();

        var userIdsToAdd = fusionAzureUserIds.Where(fusionUserId => !dbAzureUserIds.Contains(fusionUserId)).ToList();
        var userIdsToRemove = dbAzureUserIds.Where(dbUserId => !fusionAzureUserIds.Contains(dbUserId)).ToList();

        foreach (var azureAdUserId in userIdsToAdd)
        {
            context.ProjectMembers.Add(new ProjectMember
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                AzureAdUserId = azureAdUserId,
                Role = ProjectMemberRole.Observer,
                FromOrgChart = true
            });
        }

        foreach (var azureAdUserId in userIdsToRemove)
        {
            var userToRemove = pmtProjectMembers.Single(x => azureAdUserId == x.AzureAdUserId);
            context.ProjectMembers.Remove(userToRemove);
        }

        await context.SaveChangesAsync();
    }

    private async Task VerifyProjectMasterId(Guid projectId, Guid fusionContextId)
    {
        var projectProjectMasterId = await context.Projects
            .Where(p => p.Id == projectId)
            .Select(p => (Guid?)p.FusionProjectId)
            .FirstOrDefaultAsync();

        if (projectProjectMasterId == null)
        {
            throw new ProjectMasterMismatchException("ProjectMasterId not found in database");
        }

        var fusionContext = await fusionContextResolver.GetContextAsync(fusionContextId);

        var fusionProjectMasterId = fusionContext.ExternalId ?? throw new ProjectMasterMismatchException("ProjectMasterId not found in Fusion");

        if (Guid.TryParse(fusionProjectMasterId, out var fusionProjectMasterGuid) && projectProjectMasterId != fusionProjectMasterGuid)
        {
            throw new ProjectMasterMismatchException("ProjectMasterId mismatch");
        }
    }

    private async Task<List<FusionPersonDto>> GetAllPersonsOnProject(Guid fusionContextId)
    {
        var orgChartId = await TryGetOrgChartId(fusionContextId);

        if (string.IsNullOrEmpty(orgChartId))
        {
            return [];
        }

        var fusionSearchObject = new FusionSearchObject
        {
            Filter = $"positions/any(p: p/isActive eq true and p/isProjectManagementTeam eq true and p/project/id eq '{orgChartId}' and p/contract eq null) and accountClassification ne 'External'",
            Top = 100,
            Skip = 0
        };

        var fusionPeople = await GetFusionPeople(fusionSearchObject);

        return FilterToProjectManagementUsers(fusionPeople, orgChartId);
    }

    private async Task<string?> TryGetOrgChartId(Guid fusionContextId)
    {
        try
        {
            var contextRelations = await fusionContextResolver.GetContextRelationsAsync(fusionContextId);

            return contextRelations.FirstOrDefault(x => x.Type == FusionContextType.OrgChart)?.ExternalId?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private async Task<List<FusionPersonV1>> GetFusionPeople(FusionSearchObject fusionSearchObject)
    {
        try
        {
            var response = await downstreamApi.PostForUserAsync<FusionSearchObject, FusionPersonResponseV1>(
                "FusionPeople",
                fusionSearchObject,
                opt => opt.RelativePath = "search/persons/query?api-version=1.0");

            return response?.Results.Select(x => x.Document).ToList() ?? [];
        }
        catch (Exception e)
        {
            logger.LogInformation($"Exception while getting fusion people: {e.Message}");

            return [];
        }
    }

    private static List<FusionPersonDto> FilterToProjectManagementUsers(List<FusionPersonV1> fusionPeople, string orgChartId)
    {
        var pmtUsersForProject = new List<FusionPersonDto>();

        foreach (var fusionPerson in fusionPeople)
        {
            if (fusionPerson.Positions == null)
            {
                continue;
            }

            var projectPositions = fusionPerson.Positions.Where(x => x.Project != null && x.Project.Id == orgChartId).ToList();

            foreach (var projectPosition in projectPositions)
            {
                if (!IsActive(projectPosition))
                {
                    continue;
                }

                if (pmtUsersForProject.Any(x => x.AzureUniqueId == fusionPerson.AzureUniqueId))
                {
                    continue;
                }

                pmtUsersForProject.Add(new FusionPersonDto
                {
                    Name = fusionPerson.Name!,
                    Mail = fusionPerson.Mail!,
                    AzureUniqueId = fusionPerson.AzureUniqueId
                });
            }
        }

        return pmtUsersForProject;
    }

    private static bool IsActive(ApiPosition projectPosition)
    {
        return projectPosition.AppliesFrom < DateTime.UtcNow && projectPosition.AppliesTo > DateTime.UtcNow;
    }
}
