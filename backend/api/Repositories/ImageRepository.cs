using api.Context;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Dtos;

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

    public async Task<IEnumerable<ImageDto>> GetImagesByCaseIdAsync(Guid caseId)
    {
        var images = await _context.Images
            .Where(img => img.CaseId == caseId)
            .Select(img => new ImageDto
            {
                Id = img.Id,
                Url = img.Url,
                CreateTime = img.CreateTime,
                Description = img.Description,
                CaseId = img.CaseId
            })
            .ToListAsync();
        return images;
    }
}
