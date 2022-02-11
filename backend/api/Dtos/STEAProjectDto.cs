using api.Models;



namespace api.Dtos
{

    public class STEAProjectDto
    {
        public string Name { get; set; } = null!;

        public ICollection<STEACaseDto> STEACases { get; set; } = null!;

    }
}
