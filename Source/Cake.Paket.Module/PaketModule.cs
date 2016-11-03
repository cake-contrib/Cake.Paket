using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Scripting;

[assembly: CakeModule(typeof(Cake.Paket.Module.PaketModule))]

namespace Cake.Paket.Module
{
    public class PaketModule : ICakeModule
    {
        public void Register(ICakeContainerRegistrar registrar)
        {
            registrar.RegisterType<PaketScriptProcessor>().As<IScriptProcessor>().Singleton();
        }
    }
}
