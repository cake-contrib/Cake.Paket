using Cake.Core.Tooling;

namespace Cake.Paket.Addin.Push
{
    public sealed class PaketPushSettings : ToolSettings
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }

        public string EndPoint { get; set; }
    }
}
