using api.Exceptions;

using AutoMapper;

namespace api.ModelMapping;

public class MapperService(IMapper mapper, ILogger<MapperService> logger)
{
    public TDto MapToDto<T, TDto>(T entity, Guid id)
        where T : class
        where TDto : class
    {
        var dto = mapper.Map<TDto>(entity);
        if (dto == null)
        {
            var entityType = typeof(T).Name;
            logger.LogError("Mapping of {EntityType} with id {Id} resulted in a null DTO.", entityType, id);
            throw new MappingException($"Mapping of {entityType} resulted in a null DTO for id {id}");
        }
        return dto;
    }
}
