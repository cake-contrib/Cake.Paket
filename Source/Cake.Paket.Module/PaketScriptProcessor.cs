using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Tooling;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Paket.Module
{
    public sealed class PaketScriptProcessor : IScriptProcessor
    {
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly IToolLocator _tools;
        private readonly IGlobber _globber;
        private readonly ICakeConfiguration _config;
        private readonly IScriptConventions _conventions;

        public PaketScriptProcessor(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, IToolLocator tools, IGlobber globber, ICakeConfiguration config, IScriptConventions conventions)
        {
            _environment = environment;
            _log = log;
            _tools = tools;
            _globber = globber;
            _config = config;
            _conventions = conventions;
        }

        public IReadOnlyList<FilePath> InstallAddins(ScriptAnalyzerResult analyzerResult, DirectoryPath installPath)
        {
            return GetAddins(installPath);
        }

        public void InstallTools(ScriptAnalyzerResult analyzerResult, DirectoryPath installPath)
        {
            GetTools();
        }

        private IReadOnlyList<FilePath> GetAddins(DirectoryPath installPath)
        {
            installPath = installPath.MakeAbsolute(_environment);
            var pattern = installPath + @"/**/*.dll";
            var addinAssemblies = _globber.GetFiles(pattern).ToArray();

            var result = new List<FilePath>();
            foreach (var addin in addinAssemblies)
            {
                if (addin.GetFilename().ToString().Equals("Cake.Core.dll"))
                {
                    continue;
                }

                result.Add(addin);
            }

            return result;
        }

        private void GetTools()
        {
        }
    }
}
