namespace api.Models.Infrastructure.ProjectRecalculation;

public class CompletedRecalculation
{
    public int Id { get; set; }
    public required Guid ProjectId { get; set; }
    public required DateTime StartUtc { get; set; }
    public required DateTime EndUtc { get; set; }
    public required int CalculationLengthInMilliseconds { get; set; }
}
