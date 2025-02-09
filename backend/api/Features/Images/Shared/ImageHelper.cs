namespace api.Features.Images.Shared;

public static class ImageHelper
{
    public static string GetProjectBlobName(Guid projectId, Guid imageId)
    {
        return $"projects/{projectId}/{imageId}";
    }

    public static string GetCaseBlobName(Guid caseId, Guid imageId)
    {
        return $"cases/{caseId}/{imageId}";
    }
}
