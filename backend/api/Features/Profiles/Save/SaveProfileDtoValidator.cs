using System.Reflection;

using api.Exceptions;

namespace api.Features.Profiles.Save;

public static class SaveProfileDtoValidator
{
    public static void Validate(SaveTimeSeriesDto dto)
    {
        if (dto.Values == null)
        {
            throw new InvalidInputException("Values cannot be null");
        }

        if (!AllProfileTypes.Contains(dto.ProfileType))
        {
            throw new InvalidInputException($"Unknown profile type {dto.ProfileType}");
        }
    }

    public static void Validate(SaveTimeSeriesOverrideDto dto)
    {
        if (dto.Values == null)
        {
            throw new InvalidInputException("Values cannot be null");
        }

        if (!AllProfileTypes.Contains(dto.ProfileType))
        {
            throw new InvalidInputException($"Unknown profile type {dto.ProfileType}");
        }
    }

    private static readonly List<string> AllProfileTypes = typeof(ProfileTypes)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(string))
        .Select(x => (string)x.GetRawConstantValue()!)
        .ToList();
}
