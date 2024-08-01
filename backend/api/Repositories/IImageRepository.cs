using api.Models;

public interface IImageRepository
{
    Task AddImage(Image image);
    Task<List<Image>> GetImagesByCaseId(Guid caseId);
    Task<List<Image>> GetImagesByProjectId(Guid projectId);
    Task DeleteImage(Image image);
    Task<Image?> GetImageById(Guid imageId);
}
