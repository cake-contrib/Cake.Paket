using System;
using Cake.Core.Composition;
using Cake.Core.Packaging;

namespace Cake.Paket.Module
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.Paket.Module assembly.
    /// </summary>
    public sealed class PaketModule : ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registrar.
        /// </summary>
        /// <param name="registrar">The container registrar.</param>
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

#if NETCORE
            // NuGet V3
            registrar.RegisterType<V3.NuGetV3ContentResolver>().As<INuGetContentResolver>().Singleton();
#else
            // NuGet V2
            registrar.RegisterType<V2.NuGetV2ContentResolver>().As<INuGetContentResolver>().Singleton();
#endif

            // URI resource support.
            registrar.RegisterType<PaketPackageInstaller>().As<IPackageInstaller>().Singleton();
        }
    }
}
