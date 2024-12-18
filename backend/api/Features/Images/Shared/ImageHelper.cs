namespace api.Features.Images.Shared;

public static class ImageHelper
{
    public static string GetBlobName(Guid? caseId, Guid projectId, Guid imageId)
    {
        return caseId.HasValue
            ? $"cases/{caseId}/{imageId}"
            : $"projects/{projectId}/{imageId}";
    }
}
