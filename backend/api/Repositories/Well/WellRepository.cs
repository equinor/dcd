using api.Context;
using api.Models;


namespace api.Repositories;

public class WellRepository : BaseRepository, IWellRepository
{

    public WellRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await Get<Well>(wellId);
    }

    public Well UpdateWell(Well well)
    {
        return Update(well);
    }
}
