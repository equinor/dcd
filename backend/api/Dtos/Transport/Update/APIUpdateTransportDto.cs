using api.Models;
namespace api.Dtos;

public class APIUpdateTransportDto : BaseUpdateTransportDto
{
    public Maturity Maturity { get; set; }
}
