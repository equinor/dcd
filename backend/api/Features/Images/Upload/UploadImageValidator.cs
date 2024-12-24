using api.Exceptions;

namespace api.Features.Images.Upload;

public static class UploadImageValidator
{
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB
    private static readonly string[] PermittedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

    public static void EnsureIsValid(IFormFile image)
    {
        if (image.Length == 0)
        {
            throw new InputValidationException("No image provided or the file is empty.");
        }

        if (image.Length > MaxFileSize)
        {
            throw new InputValidationException($"File {image.FileName} exceeds the maximum allowed size of 5MB.");
        }

        var ext = Path.GetExtension(image.FileName).ToLower();

        if (string.IsNullOrEmpty(ext) || !PermittedExtensions.Contains(ext))
        {
            throw new InputValidationException($"File {image.FileName} has an invalid extension. Only image files are allowed.");
        }
    }
}
