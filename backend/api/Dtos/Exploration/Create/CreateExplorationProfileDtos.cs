namespace api.Dtos;

public class CreateGAndGAdminCostOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class CreateSeismicAcquisitionAndProcessingDto : CreateTimeSeriesCostDto
{
}

public class CreateCountryOfficeCostDto : CreateTimeSeriesCostDto
{
}
