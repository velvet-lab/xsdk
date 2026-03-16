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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Extensions.Package
{
    internal class Globals
    {
        internal const string SecurityContext = "vpack";
        internal const string DefaultUserAgent = "artifactory-client-dotnet";
        internal const string ApiBase = "/api";
        internal const string DetailHeaderName = "X-Result-Detail";
        public const string DefaultRepository = "repo";
        public const string DefaultLocation = "/public";
        internal const int ArtifactoryStoreOrderNumber = 99999;
    }
}
