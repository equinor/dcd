namespace api.Dtos.Project.Revision;

public class UpdateRevisionDto
{
    public string Name { get; set; } = null!;
    public bool Arena { get; set; }
    public bool Mdqc { get; set; }
}
