using api.Dtos;

namespace api.Services;

public interface ICompareCasesService
{
    Task<List<CompareCasesDto>> Calculate(Guid projectId);
}
