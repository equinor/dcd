using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateExplorationDto
{
    public string Name { get; set; } = string.Empty;
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }

    public UpdateSeismicAcquisitionAndProcessing? SeismicAcquisitionAndProcessing { get; set; }
    public UpdateCountryOfficeCost? CountryOfficeCost { get; set; }
    public UpdateExplorationWellCostProfile? ExplorationWellCostProfile { get; set; }
    public UpdateAppraisalWellCostProfile? AppraisalWellCostProfile { get; set; }
    public UpdateSidetrackCostProfile? SidetrackCostProfile { get; set; }
}

public class UpdateSeismicAcquisitionAndProcessing : UpdateTimeSeriesCostDto
{
}

public class UpdateCountryOfficeCost : UpdateTimeSeriesCostDto
{
}

public class UpdateExplorationWellCostProfile : UpdateTimeSeriesCostDto
{
}

public class UpdateAppraisalWellCostProfile : UpdateTimeSeriesCostDto
{
}

public class UpdateSidetrackCostProfile : UpdateTimeSeriesCostDto
{
}
