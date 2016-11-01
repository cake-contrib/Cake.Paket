using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Paket.Addin
{
    public class PaketSettings : ToolSettings
    {
        public string Commands { get; set; }

        public ProcessArgumentBuilder GetArguments(ICakeEnvironment environment)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append(Commands);
            return builder;
        }
    }
}
