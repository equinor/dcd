using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ISurfService
    {
        SurfDto CopySurf(Guid surfId, Guid sourceCaseId);
        ProjectDto CreateSurf(SurfDto surfDto, Guid sourceCaseId);
        ProjectDto DeleteSurf(Guid surfId);
        Surf GetSurf(Guid surfId);
        Surf NewCreateSurf(SurfDto surfDto, Guid sourceCaseId);
        SurfDto NewUpdateSurf(SurfDto updatedSurfDto);
        ProjectDto UpdateSurf(SurfDto updatedSurfDto);
    }
}
