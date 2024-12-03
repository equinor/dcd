using api.Exceptions;

namespace api.Features.Cases.Update;

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
