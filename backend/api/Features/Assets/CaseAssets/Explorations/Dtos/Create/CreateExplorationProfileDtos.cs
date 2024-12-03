using api.Dtos;

namespace api.Features.Assets.CaseAssets.Explorations.Dtos.Create;

public class CreateGAndGAdminCostOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class CreateSeismicAcquisitionAndProcessingDto : CreateTimeSeriesCostDto;

public class CreateCountryOfficeCostDto : CreateTimeSeriesCostDto;
