using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Utils;

public class VersionController : ControllerBase
{
    [HttpGet("version")]
    [AllowAnonymous]
    public string VersionNumber()
    {
        var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        return $"dcd-{attribute!.InformationalVersion[6..]}";
    }
}
