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
        var images = await _context.Images
            .Where(img => img.CaseId == caseId)
            .ToListAsync();
        return images;
    }
}