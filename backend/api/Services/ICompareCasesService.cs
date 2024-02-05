using api.Dtos;

namespace api.Services
{
    public interface ICompareCasesService
    {
        Task<IEnumerable<CompareCasesDto>> Calculate(Guid projectId);
    }
}
