namespace api.Models.Infrastructure.ProjectRecalculation;

public class PendingRecalculation
{
    public int Id { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreatedUtc { get; set; }
}
