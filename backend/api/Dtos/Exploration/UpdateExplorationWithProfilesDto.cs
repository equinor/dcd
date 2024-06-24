using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateExplorationWithProfilesDto
{
    public string Name { get; set; } = string.Empty;
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }
    public UpdateGAndGAdminCostOverrideDto? GAndGAdminCostOverride { get; set; }
    public UpdateSeismicAcquisitionAndProcessingDto? SeismicAcquisitionAndProcessing { get; set; }
    public UpdateCountryOfficeCostDto? CountryOfficeCost { get; set; }
    public UpdateExplorationWellCostProfileDto? ExplorationWellCostProfile { get; set; }
    public UpdateAppraisalWellCostProfileDto? AppraisalWellCostProfile { get; set; }
    public UpdateSidetrackCostProfileDto? SidetrackCostProfile { get; set; }
}

public class UpdateGAndGAdminCostOverrideDto : UpdateTimeSeriesCostDto
{
    public bool Override { get; set; }
}
public class UpdateGAndGAdminCostDto : UpdateTimeSeriesCostDto
{
}
public class UpdateSeismicAcquisitionAndProcessingDto : UpdateTimeSeriesCostDto
{
}

public class UpdateCountryOfficeCostDto : UpdateTimeSeriesCostDto
{
}

public class UpdateExplorationWellCostProfileDto : UpdateTimeSeriesCostDto
{
}

public class UpdateAppraisalWellCostProfileDto : UpdateTimeSeriesCostDto
{
}

public class UpdateSidetrackCostProfileDto : UpdateTimeSeriesCostDto
{
}
