using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class DevelopmentWell : IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid WellProjectId { get; set; }
    public WellProject WellProject { get; set; } = null!;

    public Guid WellId { get; set; }
    public Well Well { get; set; } = null!;

    public Guid CampaignId { get; set; }
    public Campaign Campaign { get; set; } = null!;

    public int StartYear { get; set; }
    public string InternalData { get; set; } = string.Empty;

    [NotMapped]
    public int[] Values
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
