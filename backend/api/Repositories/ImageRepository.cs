using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class ImageRepository(DcdDbContext context, ICaseRepository caseRepository) : IImageRepository
{
    public async Task AddImage(Image image)
    {
        context.Images.Add(image);

        if (image.CaseId.HasValue)
        {
            await caseRepository.UpdateModifyTime((Guid)image.CaseId);
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Image>> GetImagesByCaseId(Guid caseId)
    {
        return await context.Images
            .Where(img => img.CaseId == caseId)
            .ToListAsync();
    }

    public async Task<List<Image>> GetImagesByProjectId(Guid projectId)
    {
        return await context.Images
            .Where(img => img.ProjectId == projectId && img.CaseId == null)
            .ToListAsync();
    }

    public async Task DeleteImage(Image image)
    {
        context.Images.Remove(image);
        await context.SaveChangesAsync();
    }

    public async Task<Image?> GetImageById(Guid imageId)
    {
        return await context.Images.FindAsync(imageId);
    }
}
