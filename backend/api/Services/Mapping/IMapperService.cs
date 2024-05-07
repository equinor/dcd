namespace api.Services;

public interface IMapperService
{
    TDto MapToDto<T, TDto>(T entity, Guid id) where T : class where TDto : class;
}