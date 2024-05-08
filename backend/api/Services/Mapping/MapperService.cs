

using api.Exceptions;

using AutoMapper;

namespace api.Services;

public class MapperService : IMapperService
{
    private readonly IMapper _mapper;
    private readonly ILogger<MapperService> _logger;

    public MapperService(IMapper mapper, ILogger<MapperService> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public TDto MapToDto<T, TDto>(T entity, Guid id)
        where T : class
        where TDto : class
    {
        var dto = _mapper.Map<TDto>(entity);
        if (dto == null)
        {
            var entityType = typeof(T).Name;
            _logger.LogError("Mapping of {EntityType} with id {Id} resulted in a null DTO.", entityType, id);
            throw new MappingException($"Mapping of {entityType} resulted in a null DTO.", id);
        }
        return dto;
    }

    public T MapToEntity<T, TDto>(TDto dto, T existing, Guid id)
        where T : class
        where TDto : class
    {
        var entity = _mapper.Map(dto, existing);
        if (entity == null)
        {
            var entityType = typeof(T).Name;
            _logger.LogError("Mapping of {EntityType} with id {Id} resulted in a null entity.", entityType, id);
            throw new MappingException($"Mapping of {entityType} resulted in a null entity.", id);
        }
        return entity;
    }
}
