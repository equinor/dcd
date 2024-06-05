using api.Models;

namespace api.Dtos;

public class APIUpdateCaseWithProfilesDto : BaseUpdateCaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? ReferenceCase { get; set; }
    public ArtificialLift? ArtificialLift { get; set; }
    public ProductionStrategyOverview? ProductionStrategyOverview { get; set; }
    public int? ProducerCount { get; set; }
    public int? GasInjectorCount { get; set; }
    public int? WaterInjectorCount { get; set; }
    public double? FacilitiesAvailability { get; set; }
    public double? CapexFactorFeasibilityStudies { get; set; }
    public double? CapexFactorFEEDStudies { get; set; }
    public double? NPV { get; set; }
    public double? BreakEven { get; set; }
    public string? Host { get; set; }

    public DateTimeOffset DGADate { get; set; }
    public DateTimeOffset DGBDate { get; set; }
    public DateTimeOffset DGCDate { get; set; }
    public DateTimeOffset APXDate { get; set; }
    public DateTimeOffset APZDate { get; set; }
    public DateTimeOffset DG0Date { get; set; }
    public DateTimeOffset DG1Date { get; set; }
    public DateTimeOffset DG2Date { get; set; }
    public DateTimeOffset DG3Date { get; set; }
    public DateTimeOffset DG4Date { get; set; }

    public UpdateCessationWellsCostOverrideDto? CessationWellsCostOverride { get; set; }
    public UpdateCessationOffshoreFacilitiesCostOverrideDto? CessationOffshoreFacilitiesCostOverride { get; set; }
    public UpdateCessationOnshoreFacilitiesCostProfileDto? CessationOnshoreFacilitiesCostProfile { get; set; }

    public UpdateTotalFeasibilityAndConceptStudiesOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public UpdateTotalFEEDStudiesOverrideDto? TotalFEEDStudiesOverride { get; set; }
    public UpdateTotalOtherStudies? TotalOtherStudies { get; set; }

    public UpdateHistoricCostCostProfileDto? HistoricCostCostProfile { get; set; }
    public UpdateWellInterventionCostProfileOverrideDto? WellInterventionCostProfileOverride { get; set; }
    public UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public UpdateOnshoreRelatedOPEXCostProfileDto? OnshoreRelatedOPEXCostProfile { get; set; }
    public UpdateAdditionalOPEXCostProfileDto? AdditionalOPEXCostProfile { get; set; }

    public double Capex { get; set; }
    public CapexYear? CapexYear { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}

public class UpdateCessationWellsCostOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateCessationOffshoreFacilitiesCostOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class UpdateCessationOnshoreFacilitiesCostProfileDto : UpdateTimeSeriesCostDto
{
}


public class UpdateWellInterventionCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateTotalFeasibilityAndConceptStudiesOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateTotalFEEDStudiesOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateTotalOtherStudies : UpdateTimeSeriesCostDto
{
}

public class UpdateHistoricCostCostProfileDto : UpdateTimeSeriesCostDto
{
}
public class UpdateOnshoreRelatedOPEXCostProfileDto : UpdateTimeSeriesCostDto
{
}

public class UpdateAdditionalOPEXCostProfileDto : UpdateTimeSeriesCostDto
{
}
