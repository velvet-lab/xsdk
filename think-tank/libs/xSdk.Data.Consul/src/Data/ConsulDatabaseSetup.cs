using System;
using xSdk.Extensions.Variable;

namespace xSdk.Data
{
    public sealed class ConsulDatabaseSetup : DatabaseSetup
    {
        public ConsulDatabaseSetup()
        {
            Port = 8500;
        }

        public string Token { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string DataCenter { get; set; }

        public TimeSpan? WaitTime { get; set; }

        public bool UseTls { get; set; }

        public bool UseVaultAuth { get; set; }

        protected override void ValidateSetup()
        {
            this.ValidateMember(x => string.IsNullOrEmpty(x.Host));
            this.ValidateMember(x => !UseVaultAuth && string.IsNullOrEmpty(Token));
        }
    }
}
