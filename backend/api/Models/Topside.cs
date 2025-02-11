using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Topside : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public required double DryWeight { get; set; }
    public required double OilCapacity { get; set; }
    public required double GasCapacity { get; set; }
    public required double WaterInjectionCapacity { get; set; }
    public required ArtificialLift ArtificialLift { get; set; }
    public required Maturity Maturity { get; set; }
    public required double FuelConsumption { get; set; }
    public required double FlaredGas { get; set; }
    public required int ProducerCount { get; set; }
    public required int GasInjectorCount { get; set; }
    public required int WaterInjectorCount { get; set; }
    public required double CO2ShareOilProfile { get; set; }
    public required double CO2ShareGasProfile { get; set; }
    public required double CO2ShareWaterInjectionProfile { get; set; }
    public required double CO2OnMaxOilProfile { get; set; }
    public required double CO2OnMaxGasProfile { get; set; }
    public required double CO2OnMaxWaterInjectionProfile { get; set; }
    public required int CostYear { get; set; }
    public required DateTime? ProspVersion { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    public required Source Source { get; set; }
    public required string ApprovedBy { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
    public required double FacilityOpex { get; set; }
    public required double PeakElectricityImported { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
