using api.Models.Interfaces;

namespace api.Models.Infrastructure;

public class FrontendException : IDateTrackedEntity
{
    public int Id { get; set; }
    public required string DetailsJson { get; set; }
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}
