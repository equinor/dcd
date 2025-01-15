using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;

public class UpdateGAndGAdminCostOverrideDto : UpdateTimeSeriesCostDto
{
    public bool Override { get; set; }
}
