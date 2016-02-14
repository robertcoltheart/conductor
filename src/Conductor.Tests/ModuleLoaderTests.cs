using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Conductor
{
    [TestClass]
    public class ModuleLoaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullConstructorThrows()
        {
            new ModuleLoader(null);
        }

        [TestMethod]
        public void InitialiseTypePassedToCretaor()
        {
            var module = new MockModule();
            Type type = null;

            Func<Type, IModule> action = x =>
            {
                type = x;

                return module;
            };

            var loader = new ModuleLoader(action);
            loader.Add(typeof (object));

            loader.Initialize();

            Assert.AreEqual(typeof(object), type);
        }

        [TestMethod]
        public void InitialiseCalledOnModule()
        {
            var module = new MockModule();

            var loader = new ModuleLoader(x => module);
            loader.Add(typeof (object));

            loader.Initialize();

            Assert.IsTrue(module.InitializeCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(InitializeModuleException))]
        public void InnerErrorInInitialiseRaisesModuleInitialiseException()
        {
            var module = new InvalidModule();

            var loader = new ModuleLoader(x => module);
            loader.Add(typeof(object));

            try
            {
                loader.Initialize();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.InnerException is InvalidOperationException);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InitializeModuleException))]
        public void NullModuleReturnedThrows()
        {
            var loader = new ModuleLoader(x => null);
            loader.Add(typeof(object));

            loader.Initialize();
        }

        [TestMethod]
        public void InitializeAfterInitializedThrows()
        {
            var loader = new ModuleLoader(x => null);
            loader.Initialize();

            try
            {
                loader.Initialize();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void CreatesWithEmptyModules()
        {
            var loader = new ModuleLoader(x => null);

            Assert.IsNotNull(loader.ModuleTypes);
            Assert.IsFalse(loader.ModuleTypes.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullAddArgumentThrows()
        {
            var loader = new ModuleLoader(x => null);

            loader.Add(null);
        }

        [TestMethod]
        public void AddExistingModuleThrows()
        {
            var loader = new ModuleLoader(x => null);

            loader.Add(typeof(object));

            try
            {
                loader.Add(typeof(object));
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void AddedModuleIsVisibleInCatalog()
        {
            var loader = new ModuleLoader(x => null);

            loader.Add(typeof(object));

            Assert.IsTrue(loader.ModuleTypes.Contains(typeof(object)));
        }

        private class MockModule : IModule
        {
            public bool InitializeCalled;

            public void Initialize()
            {
                InitializeCalled = true;
            }
        }

        private class InvalidModule : IModule
        {
            public void Initialize()
            {
                throw new InvalidOperationException();
            }
        }
    }
}