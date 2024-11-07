namespace api.Dtos;

public class UpdateCessationWellsCostOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateCessationOffshoreFacilitiesCostOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class UpdateCessationOnshoreFacilitiesCostProfileDto : UpdateTimeSeriesCostDto;

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

public class UpdateTotalOtherStudiesCostProfileDto : UpdateTimeSeriesCostDto;

public class UpdateHistoricCostCostProfileDto : UpdateTimeSeriesCostDto;

public class UpdateOnshoreRelatedOPEXCostProfileDto : UpdateTimeSeriesCostDto;

public class UpdateAdditionalOPEXCostProfileDto : UpdateTimeSeriesCostDto;
