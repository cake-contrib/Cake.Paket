using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Paket.Addin.Tooling;

namespace Cake.Paket.Addin.Push
{
    internal sealed class PaketPusher : PaketTool<PaketPushSettings>
    {
        public PaketPusher(IFileSystem fileSystem, ICakeEnvironment environment, IToolLocator tools, IProcessRunner processRunner, IPaketToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools, resolver)
        {
            Environment = environment;
        }

        private ICakeEnvironment Environment { get; }

        /// <summary>
        /// Pushes NuGet package defined by the file path for the given settings.
        /// </summary>
        /// <param name="filePath">The NuGet package file path</param>
        /// <param name="settings">The settings.</param>
        internal void Push(FilePath filePath, PaketPushSettings settings)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(filePath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath filePath, PaketPushSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("push");

            // file
            builder.Append("file");
            var outputDirectory = filePath.MakeAbsolute(Environment).FullPath;
            if (!string.IsNullOrWhiteSpace(outputDirectory))
            {
                builder.AppendQuoted(filePath.MakeAbsolute(Environment).FullPath);
            }

            // url
            if (!string.IsNullOrWhiteSpace(settings.Url))
            {
                builder.Append("url");
                builder.AppendQuoted(settings.Url);
            }

            // apikey
            if (!string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                builder.Append("apikey");
                builder.AppendQuoted(settings.ApiKey);
            }

            // endpoint
            if (!string.IsNullOrWhiteSpace(settings.EndPoint))
            {
                builder.Append("endpoint");
                builder.AppendQuoted(settings.EndPoint);
            }

            return builder;
        }
    }
}
