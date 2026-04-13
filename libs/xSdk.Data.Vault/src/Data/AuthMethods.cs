/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using VaultSharp.V1.AuthMethods;

namespace xSdk.Data;

[Flags]
public enum AuthMethods
{
    None = 0,
    AppRole = 1 << 0,
    UsernamePassword = 1 << 1,
    Token = 1 << 2,
    Cert = 1 << 3
}

/*
see VaultSharp.V1.AuthMethods.AuthMethodDefaultPaths

public class AuthMethodDefaultPaths
{
    public const string AliCloud = "alicloud";
    public const string AppRole = "approle";
    public const string AWS = "aws";
    public const string Azure = "azure";
    public const string GitHub = "github";
    public const string GoogleCloud = "gcp";
    public const string JWT = "jwt";
    public const string Kubernetes = "kubernetes";
    public const string LDAP = "ldap";
    public const string Kerberos = "kerberos";
    public const string OCI = "oci";
    public const string Okta = "okta";
    public const string RADIUS = "radius";
    public const string Cert = "cert";
    public const string Token = "token";
    public const string UserPass = "userpass";
    public const string CloudFoundry = "cf";
}
*/

