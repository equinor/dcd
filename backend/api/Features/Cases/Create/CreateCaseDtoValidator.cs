using api.Exceptions;

namespace api.Features.Cases.Create;

public static class CreateCaseDtoValidator
{
    public static void Validate(CreateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new UnprocessableContentException($"{nameof(CreateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }

        if (dto.DG4Date != DateTime.MinValue)
        {
            if (dto.DG4Date < DateTime.Today.AddYears(-100))
            {
                throw new UnprocessableContentException($"{nameof(CreateCaseDto)}.{nameof(dto.DG4Date)} is outside of allowed range.");
            }

            if (dto.DG4Date > DateTime.Today.AddYears(100))
            {
                throw new UnprocessableContentException($"{nameof(CreateCaseDto)}.{nameof(dto.DG4Date)} is outside of allowed range.");
            }
        }
    }
}
