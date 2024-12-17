using api.Features.Images.Dto;

namespace api.Features.Images.Service;

public interface IBlobStorageService
{
    Task<ImageDto> SaveImage(Guid projectId, string projectName, IFormFile image, Guid? caseId = null);
    Task<byte[]> GetImageRaw(Guid projectId, Guid imageId);
    Task<List<ImageDto>> GetProjectImages(Guid projectId);
    Task DeleteImage(Guid imageId);
}
