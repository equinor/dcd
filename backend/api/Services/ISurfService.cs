using api.Models;

namespace api.Services
{
    public interface ISurfService
    {
        IEnumerable<Surf> GetSurfs(Guid projectId);
        Surf UpdateSurf(Surf surf);

        Surf CreateSurf(Surf surf);

        bool DeleteSurf(Surf surf);
    }
}
