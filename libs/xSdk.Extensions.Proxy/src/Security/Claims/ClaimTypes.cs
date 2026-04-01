using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Security.Claims;

public static class ClaimTypes
{
    private const string Context = "proxy";

    public static string Cluster => ClaimCreator.CreateClaimType(Context, "cluster");

    public static string Route => ClaimCreator.CreateClaimType(Context, "route");
}
