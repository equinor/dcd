using api.AppInfrastructure;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Cases.Update;

public class UpdateCaseController(UpdateCaseService updateCaseService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task UpdateCase(Guid projectId, Guid caseId, [FromBody] UpdateCaseDto caseDto)
    {
        if (DcdEnvironments.FeatureToggles.RevisionEnabled)
        {
            UpdateCaseDtoValidator.Validate(caseDto);
        }

        await updateCaseService.UpdateCase(projectId, caseId, caseDto);
    }
}
