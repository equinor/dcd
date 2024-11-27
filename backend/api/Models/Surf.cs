using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class Surf : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public double CessationCost { get; set; }
    public Maturity Maturity { get; set; }
    public double InfieldPipelineSystemLength { get; set; }
    public double UmbilicalSystemLength { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public int RiserCount { get; set; }
    public int TemplateCount { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ProductionFlowline ProductionFlowline { get; set; }
    public Currency Currency { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }

    public virtual SurfCostProfile? CostProfile { get; set; }
    public virtual SurfCostProfileOverride? CostProfileOverride { get; set; }
    public virtual SurfCessationCostProfile? CessationCostProfile { get; set; }
}

public class SurfCostProfile : TimeSeriesCost, ISurfTimeSeries
{
    [ForeignKey("Surf.Id")]
    public virtual Surf Surf { get; set; } = null!;
}

public class SurfCostProfileOverride : TimeSeriesCost, ISurfTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Surf.Id")]
    public virtual Surf Surf { get; set; } = null!;
    public bool Override { get; set; }
}

public class SurfCessationCostProfile : TimeSeriesCost, ISurfTimeSeries
{
    [ForeignKey("Surf.Id")]
    public virtual Surf Surf { get; set; } = null!;
}

public enum ProductionFlowline
{
    No_production_flowline,
    Carbon,
    SSClad,
    Cr13,
    Carbon_Insulation,
    SSClad_Insulation,
    Cr13_Insulation,
    Carbon_Insulation_DEH,
    SSClad_Insulation_DEH,
    Cr13_Insulation_DEH,
    Carbon_PIP,
    SSClad_PIP,
    Cr13_PIP,
    HDPELinedCS
}

public interface ISurfTimeSeries
{
    Surf Surf { get; set; }
}
