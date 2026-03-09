using System;
using Consul;

namespace xSdk.Data
{
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
                        // DevSkim: ignore DS137138
                        _.Address = new Uri($"http://{concreteSetup.Host}:{concreteSetup.Port}");
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
}
