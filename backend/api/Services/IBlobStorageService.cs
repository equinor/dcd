using api.Dtos;

public interface IBlobStorageService
{
    Task<ImageDto> SaveImage(IFormFile image, Guid caseId);
    Task<List<ImageDto>> GetImagesByCaseIdAndMapToDto(Guid caseId);
}
