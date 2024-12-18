using api.Exceptions;

namespace api.Features.Images.Upload;

public static class UploadImageValidator
{
    public static void EnsureIsValid(IFormFile image)
    {
        const int maxFileSize = 5 * 1024 * 1024; // 5MB
        string[] permittedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

        if (image.Length == 0)
        {
            throw new InputValidationException("No image provided or the file is empty.");
        }

        if (image.Length > maxFileSize)
        {
            throw new InputValidationException($"File {image.FileName} exceeds the maximum allowed size of 5MB.");
        }

        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            throw new InputValidationException($"File {image.FileName} has an invalid extension. Only image files are allowed.");
        }
    }
}
