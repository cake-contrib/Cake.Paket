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

            registrar.RegisterType<PaketPackageInstaller>().As<IPackageInstaller>().Singleton();
        }
    }
}
