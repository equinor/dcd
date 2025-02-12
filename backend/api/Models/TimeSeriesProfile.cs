using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class TimeSeriesProfile : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }
    public required string ProfileType { get; set; }

    public int StartYear { get; set; }
    public string InternalData { get; set; } = string.Empty;
    public bool Override { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

    [NotMapped]
    public required double[] Values
    {
        get => string.IsNullOrEmpty(InternalData) ? [] : Array.ConvertAll(InternalData.Split(';'), pf => (double)Convert.ChangeType(pf, typeof(double)));
        set => InternalData = string.Join(";", value);
    }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
