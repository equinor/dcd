using api.Dtos;
using api.Models;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<IEnumerable<ImageDto>> GetImagesByCaseIdAsync(Guid caseId);
}
