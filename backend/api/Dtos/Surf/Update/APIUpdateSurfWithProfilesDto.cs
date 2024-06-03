using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class APIUpdateSurfWithProfilesDto : BaseUpdateSurfDto
{
    public UpdateSurfCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
}

public class UpdateSurfCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
