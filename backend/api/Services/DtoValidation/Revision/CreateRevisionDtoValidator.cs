using api.Dtos;
using api.Exceptions;

namespace api.Services;

public static class CreateRevisionDtoValidator
{
    public static void Validate(CreateRevisionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InputValidationException($"{nameof(CreateRevisionDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }
    }
}
