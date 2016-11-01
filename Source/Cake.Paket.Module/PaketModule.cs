using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Packaging;

[assembly: CakeModule(typeof(Cake.Paket.Module.PaketModule))]

namespace Cake.Paket.Module
{
    public class PaketModule : ICakeModule
    {
        public void Register(ICakeContainerRegistrar registrar)
        {
            registrar.RegisterType<PaketContentResolver>().As<IPaketContentResolver>().Singleton();
            registrar.RegisterType<PaketPackageInstaller>().As<IPackageInstaller>().Singleton();
        }
    }
}
