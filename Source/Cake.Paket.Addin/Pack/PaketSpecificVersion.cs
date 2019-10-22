namespace Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Version number to use for package ID.
    /// </summary>
    public struct PaketSpecificVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketSpecificVersion" /> struct.
        /// </summary>
        /// <param name="packageId">Package ID.</param>
        /// <param name="version">Package version.</param>
        public PaketSpecificVersion(string packageId, string version)
        {
            PackageId = packageId;
            Version = version;
        }

        /// <summary> Gets the Package ID. </summary>
        public string PackageId { get; }

        /// <summary> Gets the Package version. </summary>
        public string Version { get; }
    }
}
