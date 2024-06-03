using api.Dtos;

public interface IBlobStorageService
{
    Task<string> GetBlobSasUrl(string blobName);
    Task<string> UploadImage(byte[] imageBytes, string contentType, string blobName);
    Task<IEnumerable<string>> GetImageUrls(Guid caseId);
    Task<ImageDto> SaveImage(IFormFile image, Guid caseId);
    Task<List<ImageDto>> GetImagesByCaseIdAndMapToDto(Guid caseId);
}
