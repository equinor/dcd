using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class ApplicationRolePolicyProvider : IAuthorizationPolicyProvider
{
    private const string PolicyPrefix = "ApplicationRoles:";
    private const string PolicyDivider = "|";


    public static string ApplicationRolesToPolicy(IEnumerable<ApplicationRole> applicationRoles)
    {
        return PolicyPrefix + string.Join(PolicyDivider, applicationRoles);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => Task.FromResult(DefaultApplicationRole());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        Task.FromResult((AuthorizationPolicy?)DefaultApplicationRole());

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var applicationRoles = PolicyToApplicationRoles(policyName).ToList();
        return Task.FromResult((AuthorizationPolicy?)PolicyWithRequiredRole(applicationRoles));
    }

    private string GetDebuggerDisplay()
    {
        if (ToString() is null)
        {
            return "";
        }
        else
        {
            return ToString(); // No warning
        }

    }

    private static IEnumerable<ApplicationRole> PolicyToApplicationRoles(string policy)
    {
        if (!policy.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"{nameof(ApplicationRolePolicyProvider)} received an unknown policy: \"{policy}\""
            );
        }


        return policy[PolicyPrefix.Length..]
            .Split(PolicyDivider)
            .Select(roleString =>
            {
                if (!Enum.IsDefined(typeof(ApplicationRole), roleString))
                {
                    throw new InvalidOperationException(
                        $"{nameof(ApplicationRolePolicyProvider)} could not recognize the application role \"{roleString}\" in the policy \"{policy}\""
                    );
                }

                return Enum.Parse<ApplicationRole>(roleString);
            });
    }

    private static AuthorizationPolicy DefaultApplicationRole()
    {
        return PolicyWithRequiredRole(new List<ApplicationRole> { ApplicationRoleRequirement.DefaultApplicationRole });
    }

    private static AuthorizationPolicy PolicyWithRequiredRole(List<ApplicationRole> requiredRoles)
    {
        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new ApplicationRoleRequirement(requiredRoles));

        return policy.Build();
    }
}
