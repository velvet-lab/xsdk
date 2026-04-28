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
using Consul;

namespace xSdk.Data;

internal sealed class ConsulConnectionBuilder : ConnectionBuilder
{
    public override object Create(IDatabaseSetup setup)
    {
        IConsulClient client = null;

        var concreteSetup = setup as ConsulDatabaseSetup;
        if (concreteSetup != null)
        {
            client = new ConsulClient(_ =>
            {
                if (concreteSetup.UseTls)
                    _.Address = new Uri($"https://{concreteSetup.Host}:{concreteSetup.Port}");
                else
                {
                    _.Address = new Uri($"http://{concreteSetup.Host}:{concreteSetup.Port}"); // DevSkim: ignore DS137138 - HTTP nur wenn UseTls=false explizit konfiguriert
                }

                if (!string.IsNullOrEmpty(concreteSetup.DataCenter))
                    _.Datacenter = concreteSetup.DataCenter;

                if (!string.IsNullOrEmpty(concreteSetup.Token))
                    _.Token = concreteSetup.Token;

                if (concreteSetup.WaitTime != null)
                    _.WaitTime = concreteSetup.WaitTime;
            });
        }

        if (client == null)
            throw new SdkException("No Connection String defined");

        return client;
    }
}
