using api.Models;

namespace api.Repositories;


public interface ICaseWithAssetsRepository
{
    Task<(
        Case CaseItem,
        DrainageStrategy DrainageStrategy,
        Topside Topside,
        Exploration Exploration,
        Substructure Substructure,
        Surf Surf,
        Transport Transport,
        WellProject WellProject
        )> GetCaseWithAssets(Guid caseId);
}
