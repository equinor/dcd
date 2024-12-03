using api.Dtos;
using api.Exceptions;
using api.Features.Cases.Update;

namespace api.Services;

public static class UpdateCaseDtoValidator
{
    public static void Validate(UpdateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InputValidationException($"{nameof(UpdateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }
    }
}
