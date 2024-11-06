using api.Models.Fusion;

using Fusion.Integration;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Abstractions;

namespace api.Services;

public interface IFusionPeopleService
{
    Task<List<FusionPersonV1>> GetAllPersonsOnProject(Guid projectMasterId, string search, int top, int skip);
}

public class FusionPeopleService(
    IDownstreamApi downstreamApi,
    IFusionContextResolver fusionContextResolver) : IFusionPeopleService
{
    public async Task<List<FusionPersonV1>> GetAllPersonsOnProject(Guid fusionContextId, string search, int top, int skip)
    {
        var contextRelations = await fusionContextResolver.GetContextRelationsAsync(fusionContextId);

        string? orgChartId = contextRelations.FirstOrDefault(x => x.Type == FusionContextType.OrgChart)?.ExternalId?.ToString();

        if (!string.IsNullOrEmpty(orgChartId))
        {
            var fusionSearchObject = BuildFusionSearchObject(orgChartId, search, top, skip);

            var result = await QueryFusionPeopleService(fusionSearchObject);
            return result.Select(x => x.Document).ToList();
        }

        return new List<FusionPersonV1>();
    }

    private static FusionSearchObject BuildFusionSearchObject(string orgChartId, string search, int top, int skip)
    {
        var recordsToSkip = top * skip;

        var fusionSearchObject = new FusionSearchObject
        {
            Filter = $"positions/any(p: p/isActive eq true and p/project/id eq '{orgChartId}' and p/contract eq null)",
            Top = top,
            Skip = recordsToSkip
        };

        if (!string.IsNullOrEmpty(search))
        {
            fusionSearchObject.Filter += $" and search.ismatch('{search}*', 'name,mail')";
        }

        return fusionSearchObject;
    }

    public async Task<List<FusionPersonResultV1>> QueryFusionPeopleService(FusionSearchObject fusionSearchObject)
    {
        var response = await downstreamApi.PostForUserAsync<FusionSearchObject, FusionPersonResponseV1>(
            "FusionPeople", fusionSearchObject,
            opt => opt.RelativePath = "search/persons/query?api-version=1.0");

        return response?.Results ?? new List<FusionPersonResultV1>();
    }
}

public class FusionSearchObject
{
    public string Filter { get; set; } = string.Empty;
    public string[] OrderBy { get; set; } = Array.Empty<string>();
    public int? Top { get; set; }
    public int? Skip { get; set; }
    public bool IncludeTotalResultCount { get; set; } = true;
    public bool MatchAll { get; set; } = true;
    public bool FullQueryMode { get; set; } = true;
}
