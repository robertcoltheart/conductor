﻿using System;
using System.Collections.Generic;
using Conductor.Properties;

namespace Conductor
{
    /// <summary>
    /// Registers types and initializes them as <see cref="IModule"/> instances.
    /// </summary>
    public class ModuleLoader : IModuleLoader, IModuleRegistry
    {
        private readonly Func<Type, IModule> moduleFactory;

        private readonly List<Type> moduleTypes = new();

        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoader"/> class.
        /// </summary>
        /// <param name="moduleFactory">The <see cref="IModule"/> creation func that returns an <see cref="IModule"/> instance from a given type.</param>
        public ModuleLoader(Func<Type, IModule> moduleFactory)
        {
            this.moduleFactory = moduleFactory ?? throw new ArgumentNullException(nameof(moduleFactory));
        }

        /// <summary>
        /// Gets all the type names that are registered.
        /// </summary>
        /// <value>The modules in the registry.</value>
        public IEnumerable<Type> ModuleTypes => moduleTypes.ToArray();

        /// <summary>
        /// Adds a module type specified by a <see cref="Type"/> to the <see cref="ModuleLoader"/>.
        /// </summary>
        /// <param name="moduleType">The module type to add.</param>
        /// <returns>The module registry.</returns>
        public IModuleRegistry Add(Type moduleType)
        {
            if (moduleType == null)
            {
                throw new ArgumentNullException(nameof(moduleType), Resources.Argument_Null);
            }

            if (moduleTypes.Contains(moduleType))
            {
                throw new InvalidOperationException(Resources.InvalidOperation_ModuleTypeExists);
            }

            moduleTypes.Add(moduleType);

            return this;
        }

        /// <summary>
        /// Loads and initializes the registered modules.
        /// </summary>
        /// <exception cref="InvalidOperationException">Modules are already initialized.</exception>
        public void Initialize()
        {
            if (initialized)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_ModulesInitialized);
            }

            foreach (var moduleType in ModuleTypes)
            {
                InitializeModule(moduleType);
            }

            initialized = true;
        }

        private void InitializeModule(Type moduleType)
        {
            var module = moduleFactory(moduleType);

            if (module == null)
            {
                throw new InitializeModuleException(string.Format(Resources.InitializeModule_NullModule, moduleType));
            }

            try
            {
                module.Initialize();
            }
            catch (Exception ex)
            {
                throw new InitializeModuleException(Resources.InitializeModule_ErrorInitializing, ex);
            }
        }
    }
}
