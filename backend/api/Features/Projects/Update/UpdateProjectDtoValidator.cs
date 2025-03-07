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
    }
}
