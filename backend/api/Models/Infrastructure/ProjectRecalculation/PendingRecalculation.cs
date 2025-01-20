namespace api.Models.Infrastructure.ProjectRecalculation;

public class PendingRecalculation
{
    public int Id { get; set; }
    public required Guid ProjectId { get; set; }
    public required DateTime CreatedUtc { get; set; }
}
