﻿using System.Collections.Generic;
using Cake.Core.Tooling;

namespace Cake.Paket.Addin.Restore
{
    /// <summary>
    /// Contains settings used by <see cref="PaketRestorer"/>. See <see
    /// href="https://fsprojects.github.io/Paket/paket-restore.html">Paket Restore</see> for more details.
    /// </summary>
    public sealed class PaketRestoreSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to cause the restore to fail if any of the checks fail.
        /// </summary>
        public bool FailOnChecks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force the download of all packages.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value to restore a single group.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip the test if paket.dependencies and
        /// paket.lock are in sync.
        /// </summary>
        public bool IgnoreChecks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to restore packages that are referenced in the
        /// paket.references files, as opposed to all the packages in paket.dependencies.
        /// </summary>
        public bool OnlyReferenced { get; set; }

        /// <summary>
        /// Gets or sets a value to restore dependencies for a project.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets the list of paket.references files paths to restore packages from.
        /// </summary>
        public List<string> ReferencesFiles { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether to touch project files referencing packages which
        /// are being restored, to help incremental build tools detecting the change.
        /// </summary>
        public bool TouchAffectedRefs { get; set; }
    }
}
