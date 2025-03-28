using System.Reflection;
using System.Text;

using api.Features.Campaigns.Update.UpdateCampaign;
using api.Features.ChangeLogs;
using api.Features.Profiles;
using api.Features.Projects.Exists;
using api.Models.Enums;

namespace api.AppInfrastructure;

public static class DcdTypescriptGenerator
{
    private static readonly List<Type> EnumTypes =
    [
        typeof(ArtificialLift),
        typeof(CampaignCostType),
        typeof(CampaignType),
        typeof(ChangeLogCategory),
        typeof(Concept),
        typeof(Currency),
        typeof(GasSolution),
        typeof(InternalProjectPhase),
        typeof(Maturity),
        typeof(NoAccessReason),
        typeof(PhysUnit),
        typeof(ProductionFlowline),
        typeof(ProductionStrategyOverview),
        typeof(ProjectCategory),
        typeof(ProjectClassification),
        typeof(ProjectMemberRole),
        typeof(ProjectPhase),
        typeof(Source),
        typeof(WellCategory)
    ];

    private static readonly List<Type> ConstantTypes =
    [
        typeof(ProfileTypes)
    ];

    public static async Task GenerateTypescriptFiles()
    {
        if (!DcdEnvironments.WriteTypescriptFiles)
        {
            return;
        }

        var builder = new StringBuilder();

        builder.AppendLine("/* This file is autogenerated by the backend. Do not modify manually. */");
        builder.AppendLine();

        builder.Append(BuildStringEnumFileContent());
        builder.Append(BuildIntEnumFileContent());

        await File.WriteAllTextAsync("../../frontend/src/Models/enums.ts", builder.ToString());
    }

    private static string BuildStringEnumFileContent()
    {
        var builder = new StringBuilder();

        foreach (var type in ConstantTypes.OrderBy(x => x.Name))
        {
            builder.AppendLine($"export enum {type.Name} {{");

            var constantValues = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue()!)
                .OrderBy(x => x)
                .ToList();

            foreach (var constantValue in constantValues)
            {
                builder.AppendLine($"    {constantValue} = \"{constantValue}\",");
            }

            builder.AppendLine("}");
        }

        return builder.ToString();
    }

    private static string BuildIntEnumFileContent()
    {
        var builder = new StringBuilder();

        foreach (var enumType in EnumTypes.OrderBy(x => x.Name))
        {
            builder.AppendLine();
            builder.AppendLine($"export enum {enumType.Name} {{");

            foreach (var enumValue in Enum.GetValues(enumType))
            {
                builder.AppendLine($"    {enumValue.ToString()} = {(int)enumValue},");
            }

            builder.AppendLine("}");
        }

        return builder.ToString();
    }
}
