using api.Dtos;
using api.Exceptions;

namespace api.Services;

public static class UpdateCaseDtoValidator
{
    public static void Validate(APIUpdateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InputValidationException($"{nameof(APIUpdateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }
    }
}
