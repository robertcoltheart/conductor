using System;
using System.Collections.Generic;

namespace Conductor
{
    /// <summary>
    /// A container for registering <see cref="IModule"/> types.
    /// </summary>
    public interface IModuleRegistry
    {
        /// <summary>
        /// Gets all the type names that are registered.
        /// </summary>
        /// <value>The modules in the registry.</value>
        IEnumerable<Type> ModuleTypes { get; }

        /// <summary>
        /// Adds a module type specified by a <see cref="Type"/> to the <see cref="IModuleRegistry"/>.
        /// </summary>
        /// <param name="moduleType">The module type to add.</param>
        /// <returns>The module registry.</returns>
        IModuleRegistry Add(Type moduleType);
    }
}