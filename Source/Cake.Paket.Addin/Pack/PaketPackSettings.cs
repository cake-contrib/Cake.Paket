using Cake.Core.Tooling;

namespace Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="PaketPacker"/>.
    /// See <see href="https://fsprojects.github.io/Paket/paket-pack.html">Paket Pack</see> for more details.
    /// </summary>
    public sealed class PaketPackSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets buildconfig.
        /// </summary>
        public string BuildConfig { get; set; }

        /// <summary>
        /// Gets or sets buildplatform.
        /// </summary>
        public string BuildPlatform { get; set; }

        /// <summary>
        /// Gets or sets version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets templatefile.
        /// </summary>
        public string TemplateFile { get; set; }

        /// <summary>
        /// Gets or sets exclude.
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        /// Gets or sets specific-version.
        /// </summary>
        public string SpecificVersion { get; set; }

        /// <summary>
        /// Gets or sets releaseNotes.
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable lock-dependencies.
        /// </summary>
        public bool LockDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable minimum-from-lock-file.
        /// </summary>
        public bool MinimumFromLockFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable pin-project-references.
        /// </summary>
        public bool PinProjectReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable symbols.
        /// </summary>
        public bool Symbols { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable include-referenced-projects.
        /// </summary>
        public bool IncludeReferencedProjects { get; set; }

        /// <summary>
        /// Gets or sets project-url.
        /// </summary>
        public string ProjectUrl { get; set; }
    }
}
