using api.Models;

namespace api.Features.Cases.Recalculation;

public static class TimeSeriesProfileValidator
{
    public static void ValidateCalculatedTimeSeries(TimeSeriesProfile profile, Guid caseId)
    {
        if (profile.StartYear < -50)
        {
            throw new TimeSeriesProfileException($"Something went wrong when calculating time series starting year - should not be {profile.StartYear} for profile of type {profile.ProfileType} for caseId {caseId}");
        }

        if (profile.StartYear > 50)
        {
            throw new TimeSeriesProfileException($"Something went wrong when calculating time series starting year - should not be {profile.StartYear} for profile of type {profile.ProfileType} for caseId {caseId}");
        }

        if (profile.Values.Length > 100)
        {
            throw new TimeSeriesProfileException($"Something went wrong when calculating time series values - should not be {profile.Values.Length} items for profile of type {profile.ProfileType} for caseId {caseId}");
        }
    }
}

public class TimeSeriesProfileException(string message) : Exception(message);
