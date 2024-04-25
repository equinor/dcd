using api.Dtos;

namespace api.Services;

public interface ITechnicalInputService
{
    Task<TechnicalInputDto> UpdateTehnicalInput(Guid projectId, UpdateTechnicalInputDto technicalInputDto);
}
