using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IDuplicateCaseService
    {
        ProjectDto DuplicateCase(Guid caseId);
        Case GetCaseNoTracking(Guid caseId);
    }
}
