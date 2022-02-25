using System;
using Xunit;

namespace Conductor
{
    public class ModuleLoaderTests
    {
        [Fact]
        public void NullConstructorThrows()
        {
            var exception = Record.Exception(() => new ModuleLoader(null!));

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void InitialiseTypePassedToCreator()
        {
            var module = new MockModule();
            Type type = null;

            IModule Action(Type x)
            {
                type = x;

                return module;
            }

            var loader = new ModuleLoader(Action);
            loader.Add(typeof (object));

            loader.Initialize();

            Assert.Equal(typeof(object), type);
        }

        [Fact]
        public void InitialiseCalledOnModule()
        {
            var module = new MockModule();

            var loader = new ModuleLoader(_ => module);
            loader.Add(typeof (object));

            loader.Initialize();

            Assert.True(module.InitializeCalled);
        }

        [Fact]
        public void InnerErrorInInitialiseRaisesModuleInitialiseException()
        {
            var module = new InvalidModule();

            var loader = new ModuleLoader(_ => module);
            loader.Add(typeof(object));

            var exception = Record.Exception(() => loader.Initialize());

            Assert.IsType<InitializeModuleException>(exception);
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }

        [Fact]
        public void NullModuleReturnedThrows()
        {
            var loader = new ModuleLoader(_ => null);
            loader.Add(typeof(object));

            var exception = Record.Exception(() => loader.Initialize());

            Assert.IsType<InitializeModuleException>(exception);
        }

        [Fact]
        public void InitializeAfterInitializedThrows()
        {
            var loader = new ModuleLoader(_ => null);
            loader.Initialize();

            var exception = Record.Exception(() => loader.Initialize());

            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void CreatesWithEmptyModules()
        {
            var loader = new ModuleLoader(_ => null);

            Assert.NotNull(loader.ModuleTypes);
            Assert.Empty(loader.ModuleTypes);
        }

        [Fact]
        public void NullAddArgumentThrows()
        {
            var loader = new ModuleLoader(_ => null);

            var exception = Record.Exception(() => loader.Add(null!));

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddExistingModuleThrows()
        {
            var loader = new ModuleLoader(_ => null);

            loader.Add(typeof(object));

            var exception = Record.Exception(() => loader.Add(typeof(object)));

            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void AddedModuleIsVisibleInCatalog()
        {
            var loader = new ModuleLoader(_ => null);

            loader.Add(typeof(object));

            Assert.Contains(typeof(object), loader.ModuleTypes);
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
