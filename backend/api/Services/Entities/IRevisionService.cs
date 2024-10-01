using api.Dtos;
using api.Models;

namespace api.Services;

public interface IRevisionService
{
    Task<string> CreateRevision(Guid projectId);
}
