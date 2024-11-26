namespace api.Features.FusionIntegration.OrgChart.Models;

public record FusionPersonResponseV1(List<FusionPersonResultV1> Results, int Count);

public record FusionPersonResultV1(FusionPersonV1 Document);

public record FusionPersonV1(
    string AzureUniqueId,
    string Mail,
    string Name,
    string AccountType,
    string AccountClassification
);
