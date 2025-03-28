using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class DrainageStrategy : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public required double NglYield { get; set; }
    public required double CondensateYield { get; set; }
    public required double GasShrinkageFactor { get; set; }
    public required int ProducerCount { get; set; }
    public required int GasInjectorCount { get; set; }
    public required int WaterInjectorCount { get; set; }
    public required ArtificialLift ArtificialLift { get; set; }
    public required GasSolution GasSolution { get; set; }

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion
}
