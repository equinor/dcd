using api.Dtos;
using api.Models;

public interface IImageRepository
{
    Task AddImage(Image image);
    Task<IEnumerable<Image>> GetImagesByCaseId(Guid caseId);
}
