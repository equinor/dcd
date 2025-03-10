using api.Exceptions;

namespace api.Features.Projects.Update;

public static class UpdateProjectDtoValidator
{
    public static void Validate(UpdateProjectDto dto)
    {
        if (dto.ExchangeRateUsdToNok <= 0)
        {
            throw new UnprocessableContentException($"{nameof(UpdateProjectDto)}.{nameof(dto.ExchangeRateUsdToNok)} must be a positive number.");
        }

        if (dto.NpvYear is < 1900 or > 2100)
        {
            throw new UnprocessableContentException($"{nameof(UpdateProjectDto)}.{nameof(dto.NpvYear)} must be near the current year.");
        }
    }
}
