using api.Dtos.Project.Revision;

namespace api.Services;

public interface IRevisionService
{
    Task<RevisionWithCasesDto> GetRevision(Guid projectId);
    Task<RevisionWithCasesDto> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto);
    Task<RevisionWithCasesDto> UpdateRevision(Guid projectId, Guid revisionId, UpdateRevisionDto updateRevisionDto);

}
