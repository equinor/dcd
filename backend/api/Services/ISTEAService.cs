using api.Dtos;

namespace api.Services
{
    public interface ISTEAService
    {
        Task<STEAProjectDto> GetInputToSTEA(Guid ProjectId);
    }
}
