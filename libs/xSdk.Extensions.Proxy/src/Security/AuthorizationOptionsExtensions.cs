using Microsoft.AspNetCore.Authorization;
using xSdk.Security.Claims;

namespace xSdk.Security;

public static class AuthorizationOptionsExtensions
{
    public static AuthorizationOptions AddProxyPolicies(this AuthorizationOptions options)
    {
        options
            .AddClusterPolicies()
            .AddRoutePolicies();

        return options;
    }

    private static AuthorizationOptions AddClusterPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Policies.CanCreateCluster, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        options.AddPolicy(Policies.CanReadCluster, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        options.AddPolicy(Policies.CanUpdateCluster, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        options.AddPolicy(Policies.CanDeleteCluster, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        return options;
    }

    private static AuthorizationOptions AddRoutePolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Policies.CanCreateRoute, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        options.AddPolicy(Policies.CanReadRoute, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        options.AddPolicy(Policies.CanUpdateRoute, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        options.AddPolicy(Policies.CanDeleteRoute, policy =>
        {
            policy
                .RequireRole("Admin");
        });

        return options;
    }
}

