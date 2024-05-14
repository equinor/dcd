using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class APIUpdateSubstructureDto : BaseUpdateSubstructureDto
{
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
