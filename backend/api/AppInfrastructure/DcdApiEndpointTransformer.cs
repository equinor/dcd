using System.Text.RegularExpressions;

namespace api.AppInfrastructure;

public partial class DcdApiEndpointTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var endpoint = value.ToString()!;
        return MyRegex().Replace(endpoint, o => $"{o.Groups[1].Value}-{o.Groups[2].Value}");
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}
