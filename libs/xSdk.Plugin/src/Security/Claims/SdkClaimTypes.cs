using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Security.Claims;

public static class SdkClaimTypes
{
    public static class ApiKey
    {
        public static string Name => ClaimCreator.CreateClaimType("apikey", "name");

        public static string Identifier => ClaimCreator.CreateClaimType("apikey", "identifier");
    }
}
