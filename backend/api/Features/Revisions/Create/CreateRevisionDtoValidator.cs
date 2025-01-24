using api.Exceptions;

namespace api.Features.Revisions.Create;

public static class CreateRevisionDtoValidator
{
    public static void Validate(CreateRevisionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new UnprocessableContentException($"{nameof(CreateRevisionDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }
    }
}
