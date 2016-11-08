// The MIT License(MIT)
//
// Copyright(c) .NET Foundation and Contributors

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    /// <summary>
    /// Represents a file locator for NuGet packages that returns relevant
    /// files for the current framework given the resource type.
    /// </summary>
    public interface INuGetContentResolver
    {
        /// <summary>
        /// Gets the relevant files for a NuGet package
        /// given a path and a resource type.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="package">The package.</param>
        /// <param name="type">The resource type.</param>
        /// <returns>A collection of files.</returns>
        IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageReference package, PackageType type);
    }
}
