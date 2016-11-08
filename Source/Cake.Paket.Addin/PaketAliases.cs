using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Paket.Addin.Pack;
using Cake.Paket.Addin.Tooling;

namespace Cake.Paket.Addin
{
    /// <summary>
    /// <para> Contains functionality for working with <see href="https://fsprojects.github.io/Paket/">paket</see>.</para>
    /// <para>
    /// In order to use the commands for this addin, you will need to include the following in your build.cake file:
    /// <code>
    /// #addin paket:?package=Cake.Paket
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("Paket")]
    public static class PaketAliases
    {
        /// <summary>
        /// Creates NuGet package(s) in the output directory for the given settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="output">The output directory.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Paket.Addin.Pack")]
        public static void PaketPack(this ICakeContext context, DirectoryPath output, PaketPackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new PaketToolResolver(context.FileSystem, context.Environment, context.Tools, context.ProcessRunner, context.Arguments, context.Log);
            var packer = new PaketPacker(context.FileSystem, context.Environment, context.Tools, context.ProcessRunner, resolver);
            packer.Pack(output, settings);
        }

        /// <summary>
        /// Creates NuGet package(s) in the output directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="output">The output directory.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        [CakeNamespaceImport("Cake.Paket.Addin.Pack")]
        public static void PaketPack(this ICakeContext context, DirectoryPath output)
        {
            PaketPack(context, output, new PaketPackSettings());
        }
    }
}
