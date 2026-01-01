using Zio;

namespace xSdk.Extensions.IO
{
    internal record RootFolders
    {
        public UPath Machine { get; set; }

        public UPath User { get; set; }

        public UPath Local { get; set; }

        public UPath MachineData { get; set; }

        public UPath UserData { get; set; }

        public UPath LocalData { get; set; }
    }
}
