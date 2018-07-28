using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.NuGet;

namespace Cake.Paket.Module
{
    /// <summary>
    /// Installer for paket URI resources.
    /// </summary>
    public sealed class PaketPackageInstaller : IPackageInstaller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPackageInstaller"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="log">The log.</param>
        public PaketPackageInstaller(ICakeEnvironment environment, INuGetContentResolver contentResolver, ICakeLog log)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ContentResolver = contentResolver ?? throw new ArgumentNullException(nameof(contentResolver));
            Log = log ?? throw new ArgumentNullException(nameof(log));
        }

        private INuGetContentResolver ContentResolver { get; }

        private ICakeEnvironment Environment { get; }

        private ICakeLog Log { get; }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <returns>
        /// <c>true</c> if this installer can install the specified resource; otherwise <c>false</c>.
        /// </returns>
        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
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

            var packagePath = GetPackagePath(path, package);
            var result = ContentResolver.GetFiles(packagePath, package, type);
            if (result.Count != 0)
            {
                return result;
            }

            if (type == PackageType.Addin)
            {
                var framework = Environment.Runtime.BuiltFramework;
                Log.Warning($"Could not find any assemblies compatible with {framework.FullName}. Perhaps you need an include parameter?");
            }
            else if (type == PackageType.Tool)
            {
                Log.Warning($"Could not find any relevant files for tool '{package.Package}'. Perhaps you need an include parameter?");
            }

            return result;
        }

        private static DirectoryPath GetFromDefaultPath(DirectoryPath path, PackageReference package)
        {
            return path.Combine(package.Package);
        }

        private static DirectoryPath GetFromGroup(DirectoryPath path, PackageReference package)
        {
            const string mainGroup = "main";
            const string key = "group";
            const string packages = "packages";

            var parameters = package.Parameters;
            if (parameters.TryGetValue(key, out var parameter))
            {
                var group = parameter.Single();
                var folders = path.Segments;
                var newFolders = new List<string>();
                foreach (var f in folders)
                {
                    if (f.Equals(packages))
                    {
                        break;
                    }

                    newFolders.Add(f);
                }

                newFolders.Add(packages);

                var packageDirectory = DirectoryPath.FromString(string.Join("/", newFolders));
                if (group.Equals(mainGroup))
                {
                    return packageDirectory;
                }

                var groupDirectory = DirectoryPath.FromString(group);
                return packageDirectory.Combine(groupDirectory);
            }

            return null;
        }

        private DirectoryPath GetPackagePath(DirectoryPath path, PackageReference package)
        {
            path = path.MakeAbsolute(Environment);

            var packagePath = GetFromGroup(path, package) ?? GetFromDefaultPath(path, package);

            return packagePath;
        }
    }
}
