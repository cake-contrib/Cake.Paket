using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Paket.Module
{
    /// <summary>
    /// Installer for paket URI resources.
    /// </summary>
    public sealed class PaketPackageInstaller : IPackageInstaller
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly INuGetContentResolver _contentResolver;
        private readonly ICakeLog _log;

        private readonly ICakeConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPackageInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="log">The log.</param>
        /// <param name="config">the configuration</param>
        public PaketPackageInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            INuGetContentResolver contentResolver,
            ICakeLog log,
            ICakeConfiguration config)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }
            if (contentResolver == null)
            {
                throw new ArgumentNullException(nameof(contentResolver));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _contentResolver = contentResolver;
            _log = log;
            _config = config;
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <returns>
        ///   <c>true</c> if this installer can install the
        ///   specified resource; otherwise <c>false</c>.
        /// </returns>
        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (package.Scheme.Equals("nuget", StringComparison.OrdinalIgnoreCase))
            {
                throw new CakeException("nuget is not supported. Perhaps you need to include the schema?");
            }

            return package.Scheme.Equals("paket", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <param name="path">The location where to install the package.</param>
        /// <returns>The installed files.</returns>
        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path = path.MakeAbsolute(_environment);

            var packagePath = path.Combine(package.Package);

            // Get the files.
            var result = _contentResolver.GetFiles(packagePath, package, type);
            if (result.Count == 0)
            {
                if (type == PackageType.Addin)
                {
                    var framework = _environment.Runtime.TargetFramework;
                    _log.Warning("Could not find any assemblies compatible with {0}. Perhaps you need an include parameter?", framework.FullName);
                }
                else if (type == PackageType.Tool)
                {
                    const string format = "Could not find any relevant files for tool '{0}'. Perhaps you need an include parameter?";
                    _log.Warning(format, package.Package);
                }
            }

            return result;
        }
    }
}
