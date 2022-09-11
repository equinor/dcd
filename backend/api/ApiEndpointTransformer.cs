using System.Text.RegularExpressions;

public class ApiEndpointTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var endpoint = value.ToString();
        return Regex.Replace(endpoint!, "([a-z])([A-Z])",
            o => string.Format("{0}-{1}", o.Groups[1].Value, o.Groups[2].Value));
    }
}
