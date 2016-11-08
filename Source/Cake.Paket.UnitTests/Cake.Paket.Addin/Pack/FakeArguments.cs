using Cake.Core;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Mock of CakeArguments class.
    /// </summary>
    internal class FakeArguments : ICakeArguments
    {
        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value.</returns>
        public string GetArgument(string name)
        {
            return name.Equals("paket") ? @".\paket" : string.Empty;
        }

        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        public bool HasArgument(string name)
        {
            return name.Equals("paket");
        }
    }
}
