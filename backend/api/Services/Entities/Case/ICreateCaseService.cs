using api.Dtos;

namespace api.Services;

public interface ICreateCaseService
{
    Task CreateCase(Guid projectId, CreateCaseDto createCaseDto);
}
