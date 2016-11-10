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

        public string BuildConfig { get; set; }

        public string BuildPlatform { get; set; }

        public string TemplateFile { get; set; }

        public string Exclude { get; set; }

        public string SpecificVersion { get; set; }

        public string ReleaseNotes { get; set; }

        public bool LockDependencies { get; set; }

        public bool MinimumFromLockFile { get; set; }

        public bool PinProjectReferences { get; set; }

        public bool Symbols { get; set; }

        public bool IncludeReferencedProjects { get; set; }

        public string ProjectUrl { get; set; }
    }
}
