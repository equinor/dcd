using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Stea.Dtos;

public class TopsideCostProfileDto : TimeSeriesCostDto;

public class TopsideCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class TopsideCessationCostProfileDto : TimeSeriesCostDto;
