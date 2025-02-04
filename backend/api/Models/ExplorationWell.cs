using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class ExplorationWell : IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid WellId { get; set; }
    public virtual Well Well { get; set; } = null!;

    public Guid ExplorationId { get; set; }
    public virtual Exploration Exploration { get; set; } = null!;

    public Guid CampaignId { get; set; }
    public virtual Campaign Campaign { get; set; } = null!;

    public int StartYear { get; set; }
    public string InternalData { get; set; } = string.Empty;

    [NotMapped]
    public int[] Values
    {
        get => string.IsNullOrEmpty(InternalData) ? [] : Array.ConvertAll(InternalData.Split(';'), pf => (int)Convert.ChangeType(pf, typeof(int)));
        set => InternalData = string.Join(";", value.Select(p => p.ToString()).ToArray());
    }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}
