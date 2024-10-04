using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICreateCaseService
{
    Task<ProjectWithAssetsDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto);
}
