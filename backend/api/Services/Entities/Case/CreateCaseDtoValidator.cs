using api.Dtos;
using api.Exceptions;

namespace api.Services;

public static class CreateCaseDtoValidator
{
    public static void Validate(CreateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InputValidationException($"{nameof(CreateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }
    }
}
