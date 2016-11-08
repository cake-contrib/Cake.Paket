using Cake.Core.IO;

namespace Cake.Paket.Addin.Tooling
{
    /// <summary>
    /// Represents a paket path resolver.
    /// </summary>
    internal interface IPaketToolResolver
    {
        /// <summary>
        /// Resolves the path to paket.exe.
        /// </summary>
        /// <returns>The path to paket.exe.</returns>
        FilePath ResolvePath();
    }
}
