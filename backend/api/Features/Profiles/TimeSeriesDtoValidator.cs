using System.Reflection;

using api.Exceptions;
using api.Features.Profiles.Dtos;

namespace api.Features.Profiles;

public static class TimeSeriesDtoValidator
{
    public static void Validate(CreateTimeSeriesDto dto)
    {
        ValidateValues(dto.Values);
        ValidateProfileType(dto.ProfileType);
    }

    public static void Validate(CreateTimeSeriesOverrideDto dto)
    {
        ValidateValues(dto.Values);
        ValidateProfileType(dto.ProfileType);
    }

    public static void Validate(UpdateTimeSeriesDto dto)
    {
        ValidateValues(dto.Values);
        ValidateProfileType(dto.ProfileType);
    }

    public static void Validate(UpdateTimeSeriesOverrideDto dto)
    {
        ValidateValues(dto.Values);
        ValidateProfileType(dto.ProfileType);
    }

    private static void ValidateValues(double[] values)
    {
        if (values == null)
        {
            throw new InvalidInputException("Values cannot be null");
        }
    }

    private static void ValidateProfileType(string profileType)
    {
        if (!AllProfileTypes.Contains(profileType))
        {
            throw new InvalidInputException($"Unknown profile type {profileType}");
        }
    }

    private static readonly List<string> AllProfileTypes = typeof(ProfileTypes)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(string))
        .Select(x => (string)x.GetRawConstantValue()!)
        .ToList();
}
