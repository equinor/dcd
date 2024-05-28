using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateExplorationDto
{
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }
}
