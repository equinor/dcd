using api.Models;

namespace api.Services;

public interface IConversionMapperService
{
    TDto MapToDto<T, TDto>(T entity, Guid id, PhysUnit physUnit) where T : class where TDto : class;
    T MapToEntity<T, TDto>(TDto dto, T existing, Guid id, PhysUnit physUnit) where T : class where TDto : class;
}
