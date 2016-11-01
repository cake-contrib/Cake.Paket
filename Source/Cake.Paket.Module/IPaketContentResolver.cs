using Cake.Core.IO;
using Cake.Core.Packaging;
using System.Collections.Generic;

namespace Cake.Paket.Module
{
    public interface IPaketContentResolver
    {
        IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageReference package, PackageType type);
    }
}
