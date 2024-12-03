using api.Exceptions;
using api.Models;

using AutoMapper;

namespace api.ModelMapping;

public class ConversionMapperService(IMapper mapper, ILogger<MapperService> logger) : IConversionMapperService
{
    public TDto MapToDto<T, TDto>(T entity, Guid id, PhysUnit physUnit)
        where T : class
        where TDto : class
    {
        var dto = mapper.Map<TDto>(entity, opts => opts.Items["ConversionUnit"] = physUnit.ToString());
        if (dto == null)
        {
            var entityType = typeof(T).Name;
            logger.LogError("Mapping of {EntityType} with id {Id} resulted in a null DTO.", entityType, id);
            throw new MappingException($"Mapping of {entityType} resulted in a null DTO.", id);
        }
        return dto;
    }

    public T MapToEntity<T, TDto>(TDto dto, T existing, Guid id, PhysUnit physUnit)
        where T : class
        where TDto : class
    {
        var entity = mapper.Map(dto, existing, opts => opts.Items["ConversionUnit"] = physUnit.ToString());
        if (entity == null)
        {
            var entityType = typeof(T).Name;
            logger.LogError("Mapping of {EntityType} with id {Id} resulted in a null entity.", entityType, id);
            throw new MappingException($"Mapping of {entityType} resulted in a null entity.", id);
        }
        return entity;
    }
}
