using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class TimeSeriesCost : IChangeTrackable
{
    public TimeSeriesCost() { }

    public TimeSeriesCost(TimeSeriesProfile? timeSeriesProfile)
    {
        if (timeSeriesProfile == null)
        {
            Id = Guid.Empty;
            StartYear = 0;
            InternalData = string.Empty;
            return;
        }

        Id = timeSeriesProfile.Id;
        StartYear = timeSeriesProfile.StartYear;
        InternalData = timeSeriesProfile.InternalData;
    }

    public Guid Id { get; set; }
    public int StartYear { get; set; }
    public string InternalData { get; set; } = string.Empty;

    [NotMapped]
    public double[] Values
    {
        get => string.IsNullOrEmpty(InternalData) ? [] : Array.ConvertAll(InternalData.Split(';'), pf => (double)Convert.ChangeType(pf, typeof(double)));
        set => InternalData = string.Join(";", value.Select(p => p.ToString()).ToArray());
    }

    public Currency Currency { get; set; }
}
