namespace api.Dtos;

public class CreateCessationWellsCostOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateCessationOffshoreFacilitiesCostOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class CreateCessationOnshoreFacilitiesCostProfileDto : CreateTimeSeriesCostDto
{
}

public class CreateWellInterventionCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateOffshoreFacilitiesOperationsCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateTotalFeasibilityAndConceptStudiesOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateTotalFEEDStudiesOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateHistoricCostCostProfileDto : CreateTimeSeriesCostDto
{
}
public class CreateOnshoreRelatedOPEXCostProfileDto : CreateTimeSeriesCostDto
{
}

public class CreateAdditionalOPEXCostProfileDto : CreateTimeSeriesCostDto
{
}
