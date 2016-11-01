using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Paket.Module
{
    public class PaketContentResolver : IPaketContentResolver
    {
        private readonly ICakeLog _log;
        private readonly IFileSystem _fileSystem;
        private readonly IGlobber _globber;

        public PaketContentResolver(ICakeLog log, IFileSystem fileSystem, IGlobber globber)
        {
            _log = log;
            _fileSystem = fileSystem;
            _globber = globber;
        }

        public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageReference package, PackageType type)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (type == PackageType.Addin)
            {
                return GetAddinAssemblies(path, package);
            }
            if (type == PackageType.Tool)
            {
                return GetToolFiles();
            }

            throw new InvalidOperationException("Unknown resource type.");
        }

        private IReadOnlyCollection<IFile> GetAddin(DirectoryPath packageDirectory, PackageReference package)
        {
            var fileSystem = new FileSystem();

            var pattern = $"{packageDirectory.FullPath}/**/{package.Package}/**/*.dll";
            var assemblies = _globber.GetFiles(pattern).ToList();

            if (!assemblies.Any())
            {
                _log.Warning("Unable to locate any assemblies under {0}", packageDirectory.FullPath);
            }
            else
            {
                return assemblies.Select(f => fileSystem.GetFile(f)).ToList();
            }

            return new List<IFile>();
        }

        private IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath packageDirectory, PackageReference package)
        {
            if (packageDirectory == null)
            {
                throw new ArgumentNullException(nameof(packageDirectory));
            }

            if (packageDirectory.IsRelative)
            {
                throw new CakeException("Package directory (" + packageDirectory.FullPath + ") must be an absolute path.");
            }

            if (!_fileSystem.Exist(packageDirectory))
            {
                return new List<IFile>();
            }

            return GetAddin(packageDirectory, package);
        }

        private IReadOnlyCollection<IFile> GetToolFiles()
        {
            _log.Warning("You don't need to include #tools aliases. These will be ignored.");
            return new List<IFile>();
        }
    }
}
