using api.Context;
using api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revision.Update;

public class UpdateRevisionService(DcdDbContext context)
{
    public async Task UpdateRevision(Guid projectId, Guid revisionId, UpdateRevisionDto updateRevisionDto)
    {
        var revision = context.Projects
                           .Include(p => p.Cases)
                           .Include(p => p.RevisionDetails)
                           .FirstOrDefault(x => x.OriginalProjectId == projectId && (x.Id == revisionId || x.FusionProjectId == revisionId))
                       ?? throw new NotFoundInDBException($"Revision with id {revisionId} not found.");

        if (revision.RevisionDetails == null)
        {
            throw new InvalidOperationException("RevisionDetails cannot be null.");
        }

        revision.RevisionDetails.RevisionName = updateRevisionDto.Name;
        revision.RevisionDetails.Arena = updateRevisionDto.Arena;
        revision.RevisionDetails.Mdqc = updateRevisionDto.Mdqc;

        await context.SaveChangesAsync();
    }
}
