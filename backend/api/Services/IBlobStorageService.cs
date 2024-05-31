using System.Threading.Tasks;

public interface IBlobStorageService
{
    Task<string> GetBlobSasUrlAsync(string blobName);
    Task<string> UploadImageAsync(byte[] imageBytes, string contentType, string blobName);
    Task<IEnumerable<string>> GetImageUrlsAsync(Guid caseId);
    Task<string> SaveImageAsync(IFormFile image);
}
