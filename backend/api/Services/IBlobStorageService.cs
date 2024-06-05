using api.Dtos;

public interface IBlobStorageService
{
    Task<ImageDto> SaveImage(Guid projectId, string projectName, IFormFile image, Guid caseId);
    Task<List<ImageDto>> GetCaseImages(Guid caseId);
}
