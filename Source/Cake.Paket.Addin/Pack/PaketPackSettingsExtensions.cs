using System;

namespace Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Contains functionality related to <see cref="PaketPackSettings"/>. See <see
    /// href="https://fsprojects.github.io/Paket/paket-pack.html">Paket Pack</see> for more details.
    /// </summary>
    public static class PaketPackSettingsExtensions
    {
        /// <summary>
        /// Excludes paket.template file by package ID; may be repeated.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageId">The excluded package ID.</param>
        /// <returns>The same <see cref="PaketPackSettings"/> instance so that multiple calls can be chained.</returns>
        public static PaketPackSettings Exclude(this PaketPackSettings settings, string packageId)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Exclusions.Add(packageId);
            return settings;
        }

        /// <summary>
        /// Pins the version of the package with given ID.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="packageId">The ID of package to pin the version.</param>
        /// <param name="version">The requested package version.</param>
        /// <returns>The same <see cref="PaketPackSettings"/> instance so that multiple calls can be chained.</returns>
        public static PaketPackSettings SpecificVersion(this PaketPackSettings settings, string packageId, string version)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.SpecificVersions.Add(new PaketSpecificVersion(packageId, version));
            return settings;
        }
    }
}
