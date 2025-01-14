using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.TechnicalInput.Dtos;

public class UpdateGAndGAdminCostOverrideDto : UpdateTimeSeriesCostDto
{
    public bool Override { get; set; }
}

public class UpdateSeismicAcquisitionAndProcessingDto : UpdateTimeSeriesCostDto;

public class UpdateCountryOfficeCostDto : UpdateTimeSeriesCostDto;
