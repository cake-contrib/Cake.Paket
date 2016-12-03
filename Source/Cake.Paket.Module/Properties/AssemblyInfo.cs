using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cake.Core.Annotations;

[assembly: InternalsVisibleTo("Cake.Paket.UnitTests")]
[assembly: CakeModule(typeof(Cake.Paket.Module.PaketModule))]
[assembly: AssemblyTitle("Cake.Paket.Module")]
[assembly: CLSCompliant(true)]
