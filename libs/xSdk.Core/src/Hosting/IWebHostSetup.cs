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

using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public interface IWebHostSetup : ISetup
{
    bool AllowSystemPorts { get; set; }

    string Bind { get; set; }

    int Grpc { get; set; }

    int Http { get; set; }

    int Https { get; set; }

    bool IsHttpsEnabled { get; }

    string TlsCertFile { get; set; }

    string TlsKeyFile { get; set; }
}
