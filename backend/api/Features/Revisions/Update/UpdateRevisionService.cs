using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Update;

public class UpdateRevisionService(DcdDbContext context)
{
    public async Task UpdateRevision(Guid revisionId, UpdateRevisionDto updateRevisionDto)
    {
        var revisionDetails = await context.RevisionDetails.SingleAsync(r => r.RevisionId == revisionId);

        revisionDetails.RevisionName = updateRevisionDto.Name;
        revisionDetails.Arena = updateRevisionDto.Arena;
        revisionDetails.Mdqc = updateRevisionDto.Mdqc;

        await context.SaveChangesAsync();
    }
}
