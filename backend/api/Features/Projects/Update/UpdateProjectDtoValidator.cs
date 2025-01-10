using api.Exceptions;

namespace api.Features.Projects.Update;

public static class UpdateProjectDtoValidator
{
    public static void Validate(UpdateProjectDto dto)
    {
        if (dto.ExchangeRateUSDToNOK <= 0)
        {
            throw new UnprocessableContentException($"{nameof(UpdateProjectDto)}.{nameof(dto.ExchangeRateUSDToNOK)} must be a positive number.");
        }
    }
}
