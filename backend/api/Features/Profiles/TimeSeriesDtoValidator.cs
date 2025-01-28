using System.Reflection;

using api.Exceptions;
using api.Features.Profiles.Dtos;

namespace api.Features.Profiles;

public static class TimeSeriesDtoValidator
{
    public static void Validate(CreateTimeSeriesDto dto)
    {
        if (dto.Values == null)
        {
            throw new InvalidInputException($"Values cannot be null on CreateTimeSeriesDto");
        }

        var allProfileTypes = GetAllProfileTypes();

        if (!allProfileTypes.Contains(dto.ProfileType))
        {
            throw new InvalidInputException($"Unknown profile type {dto.ProfileType}");
        }
    }

    public static void Validate(CreateTimeSeriesOverrideDto dto)
    {
        if (dto.Values == null)
        {
            throw new InvalidInputException($"Values cannot be null on CreateTimeSeriesOverrideDto");
        }

        var allProfileTypes = GetAllProfileTypes();

        if (!allProfileTypes.Contains(dto.ProfileType))
        {
            throw new InvalidInputException($"Unknown profile type {dto.ProfileType}");
        }
    }

    public static void Validate(UpdateTimeSeriesDto dto)
    {
        if (dto.Values == null)
        {
            throw new InvalidInputException($"Values cannot be null on UpdateTimeSeriesDto");
        }

        var allProfileTypes = GetAllProfileTypes();

        if (!allProfileTypes.Contains(dto.ProfileType))
        {
            throw new InvalidInputException($"Unknown profile type {dto.ProfileType}");
        }
    }

    public static void Validate(UpdateTimeSeriesOverrideDto dto)
    {
        if (dto.Values == null)
        {
            throw new InvalidInputException($"Values cannot be null on UpdateTimeSeriesOverrideDto");
        }

        var allProfileTypes = GetAllProfileTypes();

        if (!allProfileTypes.Contains(dto.ProfileType))
        {
            throw new InvalidInputException($"Unknown profile type {dto.ProfileType}");
        }
    }

    private static List<string> GetAllProfileTypes() => typeof(ProfileTypes)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(string))
        .Select(x => (string)x.GetRawConstantValue()!)
        .ToList();
}
