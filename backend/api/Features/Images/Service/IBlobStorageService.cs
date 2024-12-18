using api.Features.Images.Dto;

namespace api.Features.Images.Service;

public interface IBlobStorageService
{
    Task<ImageDto> SaveImage(Guid projectId, IFormFile image, Guid? caseId = null);
    Task<List<ImageDto>> GetCaseImages(Guid caseId);
    Task<List<ImageDto>> GetProjectImages(Guid projectId);
    Task DeleteImage(Guid imageId);
}
