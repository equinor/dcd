using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<IEnumerable<string>> GetImagesByCaseIdAsync(Guid caseId)
    {
        var images = await _context.Images
            .Where(img => img.CaseId == caseId)
            .Select(img => img.Url)
            .ToListAsync();
        return images;
    }
}
