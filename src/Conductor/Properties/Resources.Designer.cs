﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Conductor.Properties {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Conductor.Properties.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be null or empty..
        /// </summary>
        internal static string Argument_BlankString {
            get {
                return ResourceManager.GetString("Argument_BlankString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be null.
        /// </summary>
        internal static string Argument_Null {
            get {
                return ResourceManager.GetString("Argument_Null", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error initializing module..
        /// </summary>
        internal static string InitializeModule_ErrorInitializing {
            get {
                return ResourceManager.GetString("InitializeModule_ErrorInitializing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Module of type {0} does not implement IModule..
        /// </summary>
        internal static string InitializeModule_NullModule {
            get {
                return ResourceManager.GetString("InitializeModule_NullModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modules are already initialized..
        /// </summary>
        internal static string InvalidOperation_ModulesInitialized {
            get {
                return ResourceManager.GetString("InvalidOperation_ModulesInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Module registry already contains this type..
        /// </summary>
        internal static string InvalidOperation_ModuleTypeExists {
            get {
                return ResourceManager.GetString("InvalidOperation_ModuleTypeExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Area with name {0} not found..
        /// </summary>
        internal static string KeyNotFound_AreaNotFound {
            get {
                return ResourceManager.GetString("KeyNotFound_AreaNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot load type {0} from loaded assemblies..
        /// </summary>
        internal static string TypeLoad_CannotLoadType {
            get {
                return ResourceManager.GetString("TypeLoad_CannotLoadType", resourceCulture);
            }
        }
    }
}
