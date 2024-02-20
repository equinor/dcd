using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateExplorationDto
{
    public string Name { get; set; } = string.Empty;
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }
    public List<ExplorationWellDto>? ExplorationWells { get; set; } = [];
}
