﻿using System;
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

            // output
            // output is the default argument
            builder.AppendQuoted(output.MakeAbsolute(Environment).FullPath);

            // buildconfig
            if (!string.IsNullOrWhiteSpace(settings.BuildConfig))
            {
                builder.Append("--build-config");
                builder.AppendQuoted(settings.BuildConfig);
            }

            // buildplatform
            if (!string.IsNullOrWhiteSpace(settings.BuildPlatform))
            {
                builder.Append("--build-platform");
                builder.AppendQuoted(settings.BuildPlatform);
            }

            // templatefile
            if (!string.IsNullOrWhiteSpace(settings.TemplateFile))
            {
                builder.Append("--template");
                builder.AppendQuoted(settings.TemplateFile);
            }

            // version
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.Append("--version");
                builder.AppendQuoted(settings.Version);
            }

            // exclude
            foreach (string exclusion in settings.Exclusions)
            {
                builder.Append("--exclude");
                builder.AppendQuoted(exclusion);
            }

            // specific-version
            if (!string.IsNullOrWhiteSpace(settings.SpecificVersion))
            {
                builder.Append("--specific-version");
                builder.AppendQuoted(settings.SpecificVersion);
            }

            // ReleaseNotes
            if (!string.IsNullOrWhiteSpace(settings.ReleaseNotes))
            {
                builder.Append("--release-notes");
                builder.AppendQuoted(settings.ReleaseNotes);
            }

            // lock-dependencies
            if (settings.LockDependencies)
            {
                builder.Append("--lock-dependencies");
            }

            // minimum-from-lock-file
            if (settings.MinimumFromLockFile)
            {
                builder.Append("--minimum-from-lock-file");
            }

            // pin-project-references
            if (settings.PinProjectReferences)
            {
                builder.Append("--pin-project-references");
            }

            // symbols
            if (settings.Symbols)
            {
                builder.Append("--symbols");
            }

            // include-referenced-projects
            if (settings.IncludeReferencedProjects)
            {
                builder.Append("--include-referenced-projects");
            }

            // project-url
            if (!string.IsNullOrWhiteSpace(settings.ProjectUrl))
            {
                builder.Append("--project-url");
                builder.AppendQuoted(settings.ProjectUrl);
            }

            if (settings.InterprojectReferences != null)
            {
                builder.Append("--interproject-references");
                builder.Append(
                    GetInterprojectReferencesName((PaketInterprojectReferences)settings.InterprojectReferences));
            }

            return builder;
        }

        private string GetInterprojectReferencesName(PaketInterprojectReferences references)
        {
            switch (references)
            {
                case PaketInterprojectReferences.Min:
                    return "min";
                case PaketInterprojectReferences.Fix:
                    return "fix";
                case PaketInterprojectReferences.KeepMajor:
                    return "keep-major";
                case PaketInterprojectReferences.KeepMinor:
                    return "keep-minor";
                case PaketInterprojectReferences.KeepPatch:
                    return "keep-patch";
                default:
                    throw new ArgumentOutOfRangeException(nameof(references), references, null);
            }
        }
    }
}
