using Cake.Core.Tooling;

namespace Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="PaketPacker"/>.
    /// </summary>
    public sealed class PaketPackSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets version of the package.
        /// </summary>
        public string Version { get; set; }
    }
}
