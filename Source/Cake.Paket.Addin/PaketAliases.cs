using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Paket.Addin
{
    [CakeAliasCategory("Paket")]
    public static class PaketAliases
    {
        [CakeMethodAlias]
        public static void Paket(this ICakeContext ctx, PaketSettings settings)
        {
            var runner = new PaketRunner(ctx.FileSystem, ctx.Environment, ctx.ProcessRunner, ctx.Tools, ctx.Log);
            runner.Run(settings);
        }
    }
}
