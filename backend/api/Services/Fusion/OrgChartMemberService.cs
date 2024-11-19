using api.Exceptions;
using api.Models.Fusion;

using Fusion.Integration;

using Microsoft.Identity.Abstractions;

namespace api.Services.Fusion;

public class OrgChartMemberService(
    IDownstreamApi downstreamApi,
    IFusionContextResolver fusionContextResolver) : IOrgChartMemberService
{
    public async Task<List<FusionPersonV1>> GetAllPersonsOnProject(
        Guid fusionContextId,
        int top,
        int skip
    )
    {
        var contextRelations = await fusionContextResolver.GetContextRelationsAsync(fusionContextId);

        var orgChartId = contextRelations.FirstOrDefault(x => x.Type == FusionContextType.OrgChart)
            ?.ExternalId
            ?.ToString();

        if (string.IsNullOrEmpty(orgChartId))
        {
            throw new FusionOrgNotFoundException("OrgChart not found");
        }

        var fusionSearchObject = BuildFusionSearchObject(orgChartId, top, skip);

        var result = await QueryFusionPeopleService(fusionSearchObject);

        return result.Select(x => x.Document).ToList();
    }

    private static FusionSearchObject BuildFusionSearchObject(string orgChartId, int top, int skip)
    {
        return new FusionSearchObject
        {
            Filter = $"positions/any(p: p/isActive eq true and p/project/id eq '{orgChartId}' and p/contract eq null)",
            Top = top,
            Skip = top * skip
        };
    }

    private async Task<List<FusionPersonResultV1>> QueryFusionPeopleService(FusionSearchObject fusionSearchObject)
    {
        var response = await downstreamApi.PostForUserAsync<FusionSearchObject, FusionPersonResponseV1>(
            "FusionPeople", fusionSearchObject,
            opt => opt.RelativePath = "search/persons/query?api-version=1.0");

        return response?.Results ?? [];
    }
}

public class FusionSearchObject
{
    public string Filter { get; set; } = string.Empty;
    public string[] OrderBy { get; set; } = [];
    public int? Top { get; set; }
    public int? Skip { get; set; }
    public bool IncludeTotalResultCount { get; set; } = true;
    public bool MatchAll { get; set; } = true;
    public bool FullQueryMode { get; set; } = true;
}
