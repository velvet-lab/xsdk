using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;

namespace xSdk.Security.Claims;

public static class Policies
{
    private const string Issuer = "Proxy Authority";

    public const string CanCreateCluster = "Cluster.Create";
    public const string CanReadCluster = "Cluster.Read";
    public const string CanUpdateCluster = "Cluster.Update";
    public const string CanDeleteCluster = "Cluster.Delete";

    public const string CanCreateRoute = "Route.Create";
    public const string CanReadRoute = "Route.Read";
    public const string CanUpdateRoute = "Route.Update";
    public const string CanDeleteRoute = "Route.Delete";

    public static IReadOnlyCollection<Claim> ClaimsBundle = new ReadOnlyCollection<Claim>(new List<Claim> {

            ClaimCreator.CreateClaim(ClaimTypes.Cluster, Permissions.Create, ClaimValueTypes.String, Issuer),
            ClaimCreator.CreateClaim(ClaimTypes.Cluster, Permissions.Read, ClaimValueTypes.String, Issuer),
            ClaimCreator.CreateClaim(ClaimTypes.Cluster, Permissions.Update, ClaimValueTypes.String, Issuer),
            ClaimCreator.CreateClaim(ClaimTypes.Cluster, Permissions.Delete, ClaimValueTypes.String, Issuer),

            ClaimCreator.CreateClaim(ClaimTypes.Route, Permissions.Create, ClaimValueTypes.String, Issuer),
            ClaimCreator.CreateClaim(ClaimTypes.Route, Permissions.Read, ClaimValueTypes.String, Issuer),
            ClaimCreator.CreateClaim(ClaimTypes.Route, Permissions.Update, ClaimValueTypes.String, Issuer),
            ClaimCreator.CreateClaim(ClaimTypes.Route, Permissions.Delete, ClaimValueTypes.String, Issuer)
        });
}
