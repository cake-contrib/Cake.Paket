using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Paket.Module
{
    public sealed class PaketPackageInstaller : IPackageInstaller
    {
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly IPaketContentResolver _contentResolver;
        private readonly ICakeLog _log;
        private readonly IFileSystem _fileSystem;
        private readonly IToolLocator _tool;

        public PaketPackageInstaller(ICakeEnvironment environment, IProcessRunner processRunner, IPaketContentResolver contentResolver, ICakeLog log, IFileSystem fileSystem, IToolLocator tool)
        {
            _environment = environment;
            _processRunner = processRunner;
            _contentResolver = contentResolver;
            _log = log;
            _fileSystem = fileSystem;
            _tool = tool;
        }

        public bool CanInstall(PackageReference package, PackageType type)
        {
            return true;
        }

        private bool Try2GetAssembly(PackageReference package, PackageType type, DirectoryPath path, out IReadOnlyCollection<IFile> assembly)
        {
            path = path.MakeAbsolute(_environment);
            assembly = _contentResolver.GetFiles(path, package, type);
            if (!assembly.Any())
            {
                return false;
            }

            _log.Debug("Package {0} has been found.", package.Package);
            return true;
        }

        private IReadOnlyCollection<IFile> CannotFindAssembly(PackageReference package, PackageType type)
        {
            if (type == PackageType.Addin)
            {
                var framework = _environment.Runtime.TargetFramework;
                _log.Warning("Could not find any assemblies compatible with {0}.", framework.FullName);
            }
            else if (type == PackageType.Tool)
            {
                const string format = "Could not find any relevant files for tool '{0}'. Perhaps you need an include parameter?";
                _log.Warning(format, package.Package);
            }

            return new List<IFile>();
        }

        private IReadOnlyCollection<IFile> GetAssembly(PackageReference package, PackageType type, DirectoryPath path)
        {
            IReadOnlyCollection<IFile> assembly;

            return Try2GetAssembly(package, type, path, out assembly) ? assembly : CannotFindAssembly(package, type);
        }

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

            return GetAssembly(package, type, path);
        }
    }
}
