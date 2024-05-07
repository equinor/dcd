

using api.Exceptions;
using api.Models;

using AutoMapper;

namespace api.Services;

public class ConversionMapperService : IConversionMapperService
{
    private readonly IMapper _mapper;
    private readonly ILogger<MapperService> _logger;

    public ConversionMapperService(IMapper mapper, ILogger<MapperService> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public TDto MapToDto<T, TDto>(T entity, Guid id, PhysUnit physUnit)
        where T : class
        where TDto : class
    {
        var dto = _mapper.Map<TDto>(entity, opts => opts.Items["ConversionUnit"] = physUnit.ToString());
        if (dto == null)
        {
            var entityType = typeof(T).Name;
            _logger.LogError("Mapping of {EntityType} with id {Id} resulted in a null DTO.", entityType, id);
            throw new MappingException($"Mapping of {entityType} resulted in a null DTO.", id);
        }
        return dto;
    }
}
