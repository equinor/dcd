using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Update;

public class UpdateRevisionService(DcdDbContext context)
{
    public async Task UpdateRevision(Guid projectId, Guid revisionId, UpdateRevisionDto updateRevisionDto)
    {
        var revisionPk = await context.GetPrimaryKeyForRevisionId(revisionId);

        var revisionDetails = await context.RevisionDetails.SingleAsync(r => r.RevisionId == revisionPk);

        revisionDetails.RevisionName = updateRevisionDto.Name;
        revisionDetails.Arena = updateRevisionDto.Arena;
        revisionDetails.Mdqc = updateRevisionDto.Mdqc;

        await context.SaveChangesAsync();
    }
}
