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

    public async Task AddImageAsync(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Image>> GetImagesByCaseIdAsync(Guid caseId)
    {
        return await _context.Images
            .Where(img => img.CaseId == caseId)
            .ToListAsync();
    }
}
