using api.Exceptions;
using api.Features.Cases.Update;

namespace api.Features.Cases.Create;

public static class CreateCaseDtoValidator
{
    public static void Validate(CreateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new UnprocessableContentException($"{nameof(CreateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }

        if (dto.Dg4Date < UpdateCaseDtoValidator.MinAllowedDgDate)
        {
            throw new UnprocessableContentException($"{nameof(CreateCaseDto)}.{nameof(dto.Dg4Date)} is outside of allowed range.");
        }

        if (dto.Dg4Date > UpdateCaseDtoValidator.MaxAllowedDbDate)
        {
            throw new UnprocessableContentException($"{nameof(CreateCaseDto)}.{nameof(dto.Dg4Date)} is outside of allowed range.");
        }
    }
}
