namespace Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Referenced project versions constraint used when overriding paket.template constraints.
    /// </summary>
    public enum PaketInterprojectReferences
    {
        /// <summary> Version constraint like: 1.2.3 </summary>
        Min,

        /// <summary> Version constraint like: [1.2.3] </summary>
        Fix,

        /// <summary> Version constraint like: [1.2.3,2.0.0) </summary>
        KeepMajor,

        /// <summary> Version constraint like: [1.2.3,1.3.0) </summary>
        KeepMinor,

        /// <summary> Version constraint like: [1.2.3,1.2.4) </summary>
        KeepPatch,
    }
}
