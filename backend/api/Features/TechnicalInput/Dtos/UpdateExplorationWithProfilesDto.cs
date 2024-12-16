using api.Features.CaseProfiles.Dtos.TimeSeries.Update;
using api.Models;

namespace api.Features.TechnicalInput.Dtos;

public class UpdateGAndGAdminCostOverrideDto : UpdateTimeSeriesCostDto
{
    public bool Override { get; set; }
}

public class UpdateSeismicAcquisitionAndProcessingDto : UpdateTimeSeriesCostDto;

public class UpdateCountryOfficeCostDto : UpdateTimeSeriesCostDto;

public class UpdateExplorationWellCostProfileDto : UpdateTimeSeriesCostDto;

public class UpdateAppraisalWellCostProfileDto : UpdateTimeSeriesCostDto;

public class UpdateSidetrackCostProfileDto : UpdateTimeSeriesCostDto;
