using api.Dtos;

public interface IBlobStorageService
{
    Task<string> GetBlobSasUrlAsync(string blobName);
    Task<string> UploadImageAsync(byte[] imageBytes, string contentType, string blobName);
    Task<IEnumerable<string>> GetImageUrlsAsync(Guid caseId);
    Task<ImageDto> SaveImageAsync(IFormFile image, Guid caseId);
}
