using api.Models;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<List<Image>> GetImagesByCaseIdAsync(Guid caseId);
}