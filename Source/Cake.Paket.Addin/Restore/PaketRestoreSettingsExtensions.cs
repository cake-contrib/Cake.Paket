using System;

namespace Cake.Paket.Addin.Restore
{
    /// <summary>
    /// Contains functionality related to <see cref="PaketRestoreSettings"/>. See <see
    /// href="https://fsprojects.github.io/Paket/paket-restore.html">Paket Restore</see> for more details.
    /// </summary>
    public static class PaketRestoreSettingsExtensions
    {
        /// <summary>
        /// Restore packages from a paket.references file; may be repeated.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="path">The path of paket.references file, from which to restore files.</param>
        /// <returns>The same <see cref="PaketRestoreSettings"/> instance so that multiple calls can be chained.</returns>
        public static PaketRestoreSettings WithReferencesFile(this PaketRestoreSettings settings, string path)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ReferencesFiles.Add(path);
            return settings;
        }
    }
}
