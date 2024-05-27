
using api.Models;
namespace api.Dtos;

public class APIUpdateSurfDto : BaseUpdateSurfDto
{
    public Maturity Maturity { get; set; }
}

