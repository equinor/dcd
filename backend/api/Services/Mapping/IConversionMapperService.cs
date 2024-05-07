using api.Models;

namespace api.Services;

public interface IConversionMapperService
{
    TDto MapToDto<T, TDto>(T entity, Guid id, PhysUnit physUnit) where T : class where TDto : class;
}
