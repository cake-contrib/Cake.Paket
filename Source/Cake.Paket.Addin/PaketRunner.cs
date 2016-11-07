using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Paket.Addin
{
    public sealed class PaketRunner : Tool<PaketSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolLocator _toolLocator;
        private readonly IProcessRunner _process;
        private readonly ICakeLog _log;

        public PaketRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator, ICakeLog log) : base(fileSystem, environment, processRunner, toolLocator)
        {
            _environment = environment;
            _toolLocator = toolLocator;
            _process = processRunner;
            _log = log;
        }

        public void Run(PaketSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, settings.GetArguments(_environment));
        }

        protected override string GetToolName() => "Paket";

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return @"paket.exe";
        }
    }
}
