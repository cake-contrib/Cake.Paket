using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Paket.Addin.Tooling
{
    /// <summary>
    /// Contains paket path resolver functionality.
    /// </summary>
    internal sealed class PaketToolResolver : IPaketToolResolver
    {
        private IFile _cachedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaketToolResolver" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="log">The log.</param>
        internal PaketToolResolver(IFileSystem fileSystem, ICakeEnvironment environment, IToolLocator tools, IProcessRunner processRunner, ICakeArguments arguments, ICakeLog log)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Tools = tools ?? throw new ArgumentNullException(nameof(tools));
            ProcessRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            Log = log ?? throw new ArgumentNullException(nameof(log));
        }

        private IFileSystem FileSystem { get; }

        private ICakeEnvironment Environment { get; }

        private IToolLocator Tools { get; }

        private IProcessRunner ProcessRunner { get; }

        private ICakeArguments Arguments { get; }

        private ICakeLog Log { get; }

        /// <summary>
        /// Resolves the path to paket.exe.
        /// </summary>
        /// <returns>The path to paket.exe.</returns>
        public FilePath ResolvePath()
        {
            return ResolvePath2PaketExe();
        }

        private DirectoryPath Try2GetDotPaket(out bool wasResolved)
        {
            // Try to get it from the enviornment variable
            var paketExeEnviornmentVariable = Environment.GetEnvironmentVariable("PAKET");
            if (!string.IsNullOrWhiteSpace(paketExeEnviornmentVariable))
            {
                wasResolved = true;
                return new DirectoryPath(paketExeEnviornmentVariable);
            }

            // Try to get it from arguments passed into Cake.exe --paket "..."
            if (Arguments.HasArgument("paket"))
            {
                var paketExeArgument = Arguments.GetArgument("paket");
                wasResolved = true;
                return new DirectoryPath(paketExeArgument);
            }

            // See if .paket exits in the same directory as the cake script.
            var cakeScriptDirectory = Environment.WorkingDirectory;
            var paket = cakeScriptDirectory.CombineWithFilePath(new FilePath(".paket"));
            var p = FileSystem.GetFile(paket);
            if (p.Exists)
            {
                wasResolved = true;
                return new DirectoryPath(p.Path.FullPath);
            }

            wasResolved = false;
            return null;
        }

        private FilePath Try2GetExeInDotPaket(string exe, out bool wasResolved)
        {
            var dotPaket = Try2GetDotPaket(out var gotDotPaket);
            if (gotDotPaket)
            {
                var e = FileSystem.GetFile(dotPaket.CombineWithFilePath(new FilePath(exe)));
                if (e.Exists)
                {
                    wasResolved = true;
                    return e.Path;
                }
            }

            wasResolved = false;
            return null;
        }

        private FilePath Try2ResolvePath2PaketExe(out bool wasResolved)
        {
            var paketExe = Try2GetExeInDotPaket("paket.exe", out var gotIt);
            if (gotIt)
            {
                wasResolved = true;
                return paketExe;
            }

            wasResolved = false;
            return null;
        }

        private FilePath Try2ResolvePath2PaketBootStrapperExe(out bool wasResolved)
        {
            // Check if path already resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                wasResolved = true;
                return _cachedPath.Path;
            }

            // Try to resolve it with the regular tool resolver.
            var toolsExe = Tools.Resolve("paket.bootstrapper.exe");
            if (toolsExe != null)
            {
                var toolsFile = FileSystem.GetFile(toolsExe);
                if (toolsFile.Exists)
                {
                    _cachedPath = toolsFile;
                    wasResolved = true;
                    return _cachedPath.Path;
                }
            }

            var paketBootStrapperExe = Try2GetExeInDotPaket("paket.bootstrapper.exe", out var gotIt);
            if (gotIt)
            {
                wasResolved = true;
                return paketBootStrapperExe;
            }

            wasResolved = false;
            return null;
        }

        private bool RunPaketBootStrapperExe(FilePath paketBootStrapperExe)
        {
            var process = ProcessRunner.Start(paketBootStrapperExe, new ProcessSettings { RedirectStandardOutput = true, Silent = Log.Verbosity < Verbosity.Diagnostic });
            process.WaitForExit();

            var exitCode = process.GetExitCode();
            if (exitCode == 0)
            {
                return true;
            }

            Log.Warning("Paket exited with {0}", exitCode);
            var output = string.Join(System.Environment.NewLine, process.GetStandardOutput());
            Log.Verbose(Verbosity.Diagnostic, "Output:\r\n{0}", output);
            return false;
        }

        private FilePath Try2ResolvePath2PaketExeByRunningPaketBootStrapperExeFirst(out bool wasResolved)
        {
            var paketBootStrapperExe = Try2ResolvePath2PaketBootStrapperExe(out var paketBootStrapperExeWasResolved);
            if (paketBootStrapperExeWasResolved)
            {
                if (RunPaketBootStrapperExe(paketBootStrapperExe))
                {
                    var paketExe = Try2ResolvePath2PaketExe(out var paketExeWasResolved);
                    if (paketExeWasResolved)
                    {
                        wasResolved = true;
                        return paketExe;
                    }
                }
            }

            wasResolved = false;
            return null;
        }

        private FilePath ResolvePath2PaketExe()
        {
            var paketExe = Try2ResolvePath2PaketExe(out var paketExeWasResolved);
            if (paketExeWasResolved)
            {
                return paketExe;
            }

            paketExe = Try2ResolvePath2PaketExeByRunningPaketBootStrapperExeFirst(out paketExeWasResolved);
            if (paketExeWasResolved)
            {
                return paketExe;
            }

            throw new CakeException("Could not locate paket.exe.");
        }
    }
}
