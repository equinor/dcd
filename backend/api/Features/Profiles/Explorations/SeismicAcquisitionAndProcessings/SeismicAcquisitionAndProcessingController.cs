using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings;

public class SeismicAcquisitionAndProcessingController(SeismicAcquisitionAndProcessingService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/seismic-acquisition-and-processing")]
    public async Task<TimeSeriesCostDto> CreateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await service.CreateSeismicAcquisitionAndProcessing(projectId, caseId, explorationId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/seismic-acquisition-and-processing/{costProfileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await service.UpdateSeismicAcquisitionAndProcessing(projectId, caseId, explorationId, costProfileId, dto);
    }
}
