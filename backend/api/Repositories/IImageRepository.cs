using api.Dtos;
using api.Models;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<IEnumerable<Image>> GetImagesByCaseIdAsync(Guid caseId);
}
