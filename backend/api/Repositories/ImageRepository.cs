using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

public class ImageRepository : IImageRepository
{
    private readonly DcdDbContext _context;

    public ImageRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task AddImage(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Image>> GetImagesByCaseId(Guid caseId)
    {
        return await _context.Images
            .Where(img => img.CaseId == caseId)
            .ToListAsync();
    }

    public async Task<List<Image>> GetImagesByProjectId(Guid projectId)
    {
        return await _context.Images
            .Where(img => img.ProjectId == projectId && img.CaseId == null)
            .ToListAsync();
    }

    public async Task DeleteImage(Image image)
    {
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
    }

    public async Task<Image?> GetImageById(Guid imageId)
    {
        return await _context.Images.FindAsync(imageId);
    }
}
