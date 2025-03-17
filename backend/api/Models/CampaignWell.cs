using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class CampaignWell : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid WellId { get; set; }
    public Well Well { get; set; } = null!;

    public Guid CampaignId { get; set; }
    public Campaign Campaign { get; set; } = null!;

    public required int StartYear { get; set; }
    public string InternalData { get; set; } = string.Empty;

    [NotMapped]
    public required int[] Values
    {
        get => string.IsNullOrEmpty(InternalData) ? [] : Array.ConvertAll(InternalData.Split(';'), pf => (int)Convert.ChangeType(pf, typeof(int)));
        set => InternalData = string.Join(";", value.Select(p => p.ToString()).ToArray());
    }

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion
}
