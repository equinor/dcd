using api.Models;

namespace api.Dtos;

public class APIUpdateTopsideDto : BaseUpdateTopsideDto
{
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
