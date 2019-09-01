using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Paket.Addin.Tooling;

namespace Cake.Paket.Addin.Restore
{
    /// <summary>
    /// The paket restorer.
    /// </summary>
    internal sealed class PaketRestorer : PaketTool<PaketRestoreSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketRestorer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The enviornment.</param>
        /// <param name="toolLocator">The tool locator.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="paketToolResolver">The paket tool resolver.</param>
        internal PaketRestorer(IFileSystem fileSystem, ICakeEnvironment environment, IToolLocator toolLocator, IProcessRunner processRunner, IPaketToolResolver paketToolResolver)
            : base(fileSystem, environment, processRunner, toolLocator, paketToolResolver)
        {
        }

        /// <summary>
        /// Runs paket restore for the given settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        internal void Restore(PaketRestoreSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(PaketRestoreSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("restore");

            // force
            if (settings.Force)
            {
                builder.Append("--force");
            }

            // only-referenced
            if (settings.OnlyReferenced)
            {
                builder.Append("--only-referenced");
            }

            // touch-affected-refs
            if (settings.TouchAffectedRefs)
            {
                builder.Append("--touch-affected-refs");
            }

            // ignore-checks
            if (settings.IgnoreChecks)
            {
                builder.Append("--ignore-checks");
            }

            // fail-on-checks
            if (settings.FailOnChecks)
            {
                builder.Append("--fail-on-checks");
            }

            // group
            if (!string.IsNullOrWhiteSpace(settings.Group))
            {
                builder.Append("--group");
                builder.AppendQuoted(settings.Group);
            }

            // project
            if (!string.IsNullOrWhiteSpace(settings.Project))
            {
                builder.Append("--project");
                builder.AppendQuoted(settings.Project);
            }

            // references-files
            if (!string.IsNullOrWhiteSpace(settings.ReferencesFiles))
            {
                builder.Append("--references-files");
                builder.AppendQuoted(settings.ReferencesFiles);
            }

            return builder;
        }
    }
}
