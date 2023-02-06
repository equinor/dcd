using api.Dtos;

namespace api.Services
{
    public interface ICompareCasesService
    {
        IEnumerable<CompareCasesDto> Calculate(Guid projectId);
    }
}
