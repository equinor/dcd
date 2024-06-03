using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<IEnumerable<string>> GetImagesByCaseIdAsync(Guid caseId);
}
