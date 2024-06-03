using api.Dtos;
using api.Models;

public interface IImageRepository
{
    Task AddImage(Image image);
    Task<List<Image>> GetImagesByCaseId(Guid caseId);
}
