namespace api.Features.Revisions.Update;

public class UpdateRevisionDto
{
    public string Name { get; set; } = null!;
    public bool Arena { get; set; }
    public bool Mdqc { get; set; }
}
