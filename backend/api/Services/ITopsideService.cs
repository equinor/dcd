using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ITopsideService
    {
        TopsideDto CopyTopside(Guid topsideId, Guid sourceCaseId);
        ProjectDto CreateTopside(TopsideDto topsideDto, Guid sourceCaseId);
        ProjectDto DeleteTopside(Guid topsideId);
        Topside GetTopside(Guid topsideId);
        Topside NewCreateTopside(TopsideDto topsideDto, Guid sourceCaseId);
        TopsideDto NewUpdateTopside(TopsideDto updatedTopsideDto);
        ProjectDto UpdateTopside(TopsideDto updatedTopsideDto);
    }
}
