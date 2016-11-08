using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Paket.Addin.Tooling;

namespace Cake.Paket.Addin.Pack
{
    /// <summary>
    /// The paket packer.
    /// </summary>
    internal sealed class PaketPacker : PaketTool<PaketPackSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The enviornment.</param>
        /// <param name="toolLocator">The tool locator.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="paketToolResolver">The paket tool resolver.</param>
        internal PaketPacker(IFileSystem fileSystem, ICakeEnvironment environment, IToolLocator toolLocator, IProcessRunner processRunner, IPaketToolResolver paketToolResolver)
            : base(fileSystem, environment, processRunner, toolLocator, paketToolResolver)
        {
            Environment = environment;
        }

        private ICakeEnvironment Environment { get; }

        /// <summary>
        /// Creates NuGet package(s) in the output directory for the given settings.
        /// </summary>
        /// <param name="output">The output directory.</param>
        /// <param name="settings">The settings.</param>
        internal void Pack(DirectoryPath output, PaketPackSettings settings)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(output, settings));
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath output, PaketPackSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("pack");

            // Output directory
            builder.Append("output");
            var outputDirectory = output.MakeAbsolute(Environment).FullPath;
            if (!string.IsNullOrWhiteSpace(outputDirectory))
            {
                builder.AppendQuoted(output.MakeAbsolute(Environment).FullPath);
            }

            // Version
            builder.Append("version");
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.AppendQuoted(settings.Version);
            }

            return builder;
        }
    }
}
