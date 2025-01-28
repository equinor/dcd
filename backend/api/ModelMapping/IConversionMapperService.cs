using api.Models;

namespace api.ModelMapping;

public interface IConversionMapperService
{
    public TDto MapToDto<T, TDto>(T entity, PhysUnit physUnit) where T : class where TDto : class;
    TDto MapToDto<T, TDto>(T entity, Guid id, PhysUnit physUnit) where T : class where TDto : class;
    T MapToEntity<T, TDto>(TDto dto, T existing, Guid id, PhysUnit physUnit) where T : class where TDto : class;
}
