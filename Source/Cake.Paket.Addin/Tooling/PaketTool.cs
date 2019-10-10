using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Paket.Addin.Tooling
{
    /// <summary>
    /// Base class for all paket related tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    internal class PaketTool<TSettings> : Tool<TSettings>
        where TSettings : ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The paket tool resolver.</param>
        protected PaketTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IPaketToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            Resolver = resolver;
        }

        private IPaketToolResolver Resolver { get; }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected sealed override string GetToolName()
        {
            return "Paket";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected sealed override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "paket.exe";
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected sealed override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            var path = Resolver.ResolvePath();
            return new[] { path };
        }
    }
}
