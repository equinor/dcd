using api.Dtos;

namespace api.Services
{
    public interface ISTEAService
    {
        STEAProjectDto GetInputToSTEA(Guid ProjectId);
    }
}
