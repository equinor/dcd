using System.Text.RegularExpressions;

namespace api.StartupConfiguration;

public class DcdApiEndpointTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var endpoint = value.ToString();
        return Regex.Replace(endpoint!, "([a-z])([A-Z])", o => $"{o.Groups[1].Value}-{o.Groups[2].Value}");
    }
}
