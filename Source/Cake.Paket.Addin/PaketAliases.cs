using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Paket.Addin.Pack;
using Cake.Paket.Addin.Push;
using Cake.Paket.Addin.Restore;
using Cake.Paket.Addin.Tooling;

namespace Cake.Paket.Addin
{
    /// <summary>
    /// <para>Contains functionality for working with <see href="https://fsprojects.github.io/Paket/">paket</see>.</para>
    /// <para>
    /// In order to use the commands for this addin, you will need to include the following in your
    /// build.cake file:
    /// <code>
    /// #addin paket:?package=Cake.Paket
    /// </code>
    /// </para>
    /// <para>
    /// This assumes your using the <see
    /// href="https://www.nuget.org/packages/Cake.Paket.Module/">Cake.Paket.Module</see>. If you'd
    /// rather use NuGet then include:
    /// <code>
    /// #addin nuget:?package=Cake.Paket
    /// #tool nuget:?package=Paket
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
            var resolver = GetPaketToolResolver(context);
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

        /// <summary>
        /// Pushes NuGet package defined by the file path for the given settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The NuGet package file path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Paket.Addin.Push")]
        public static void PaketPush(this ICakeContext context, FilePath filePath, PaketPushSettings settings)
        {
            PaketPush(context, new[] { filePath }, settings);
        }

        /// <summary>
        /// Pushes NuGet packages defined by the file paths for the given settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The NuGet packages file path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        [CakeNamespaceImport("Cake.Paket.Addin.Push")]
        public static void PaketPush(this ICakeContext context, IEnumerable<FilePath> filePaths, PaketPushSettings settings)
        {
            var resolver = GetPaketToolResolver(context);
            var packer = new PaketPusher(context.FileSystem, context.Environment, context.Tools, context.ProcessRunner, resolver);

            foreach (var filePath in filePaths)
            {
                packer.Push(filePath, settings);
            }
        }

        /// <summary>
        /// Runs paket restore for the given settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Paket.Addin.Restore")]
        public static void PaketRestore(this ICakeContext context, PaketRestoreSettings settings)
        {
            var resolver = GetPaketToolResolver(context);
            var restorer = new PaketRestorer(context.FileSystem, context.Environment, context.Tools, context.ProcessRunner, resolver);
            restorer.Restore(settings);
        }

        /// <summary>
        /// Runs paket restore for the given settings.
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Restore")]
        [CakeNamespaceImport("Cake.Paket.Addin.Restore")]
        public static void PaketRestore(this ICakeContext context)
        {
            PaketRestore(context, new PaketRestoreSettings());
        }

        private static PaketToolResolver GetPaketToolResolver(ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new PaketToolResolver(context.FileSystem, context.Environment, context.Tools, context.ProcessRunner, context.Arguments, context.Log);
        }
    }
}
