namespace api.Models.Infrastructure.ProjectRecalculation;

public class CompletedRecalculation
{
    public int Id { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public int CalculationLengthInMilliseconds { get; set; }
}
