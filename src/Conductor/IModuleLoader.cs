using System;

namespace Conductor
{
    /// <summary>
    /// Initializes <see cref="IModule"/> types that are registered.
    /// </summary>
    public interface IModuleLoader
    {
        /// <summary>
        /// Loads and initializes the registered modules.
        /// </summary>
        /// <exception cref="InvalidOperationException">Modules are already initialized.</exception>
        void Initialize();
    }
}