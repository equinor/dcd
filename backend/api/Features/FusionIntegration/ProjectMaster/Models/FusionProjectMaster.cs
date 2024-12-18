using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace api.Features.FusionIntegration.ProjectMaster.Models;

[Serializable]
[DataContract]
public class FusionProjectMaster
{
    [DataMember]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [DataMember]
    [JsonPropertyName("identity")]
    public Guid Identity { get; init; }

    [DataMember]
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [DataMember]
    [JsonPropertyName("projectState")]
    public string? ProjectState { get; init; }

    [DataMember]
    [JsonPropertyName("portfolioOrganizationalUnit")]
    public string? PortfolioOrganizationalUnit { get; init; }

    [DataMember]
    [JsonPropertyName("organizationalUnit")]
    public string? OrganizationalUnit { get; init; }

    [DataMember]
    [JsonPropertyName("cvpid")]
    public string? Cvpid { get; init; }

    [DataMember]
    [JsonPropertyName("phase")]
    public string? Phase { get; init; }

    [DataMember]
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [DataMember]
    [JsonPropertyName("projectCategory")]
    public string? ProjectCategory { get; init; }

    [DataMember]
    [JsonPropertyName("isValid")]
    public bool? IsValid { get; init; }

    [DataMember]
    [JsonPropertyName("facilities")]
    public List<string>? Facilities { get; init; }

    [DataMember]
    [JsonPropertyName("alternateNames")]
    public List<string>? AlternateNames { get; init; } = [];

    [DataMember]
    [JsonPropertyName("documentManagementId")]
    public string? DocumentManagementId { get; init; }
}
