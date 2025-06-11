using TUnit.Assertions.AssertConditions.Throws;

namespace Conductor.Tests
{
    public class ModuleLoaderTests
    {
        [Test]
        public async Task NullConstructorThrows()
        {
            await Assert.That(() => new ModuleLoader(null!)).Throws<ArgumentNullException>();
        }

        [Test]
        public async Task InitialiseTypePassedToCreator()
        {
            var module = new MockModule();
            Type type = null!;

            IModule Action(Type x)
            {
                type = x;

                return module;
            }

            var loader = new ModuleLoader(Action);
            loader.Add(typeof (object));

            loader.Initialize();

            await Assert.That(type).IsEqualTo(typeof(object));
        }

        [Test]
        public async Task InitialiseCalledOnModule()
        {
            var module = new MockModule();

            var loader = new ModuleLoader(_ => module);
            loader.Add(typeof (object));

            loader.Initialize();

            await Assert.That(module.InitializeCalled).IsTrue();
        }

        [Test]
        public async Task InnerErrorInInitialiseRaisesModuleInitialiseException()
        {
            var module = new InvalidModule();

            var loader = new ModuleLoader(_ => module);
            loader.Add(typeof(object));

            var exception = Assert.Throws(() => loader.Initialize());

            await Assert.That(exception).IsTypeOf<InitializeModuleException>();
            await Assert.That(exception.InnerException).IsTypeOf<InvalidOperationException>();
        }

        [Test]
        public async Task NullModuleReturnedThrows()
        {
            var loader = new ModuleLoader(_ => null!);
            loader.Add(typeof(object));

            var exception = Assert.Throws(() => loader.Initialize());

            await Assert.That(exception).IsTypeOf<InitializeModuleException>();
        }

        [Test]
        public async Task InitializeAfterInitializedThrows()
        {
            var loader = new ModuleLoader(_ => null!);
            loader.Initialize();

            var exception = Assert.Throws(() => loader.Initialize());

            await Assert.That(exception).IsTypeOf<InvalidOperationException>();
        }

        [Test]
        public async Task CreatesWithEmptyModules()
        {
            var loader = new ModuleLoader(_ => null!);

            await Assert.That(loader.ModuleTypes).IsNotNull().And.IsEmpty();
        }

        [Test]
        public async Task NullAddArgumentThrows()
        {
            var loader = new ModuleLoader(_ => null!);

            var exception = Assert.Throws(() => loader.Add(null!));

            await Assert.That(exception).IsTypeOf<ArgumentNullException>();
        }

        [Test]
        public async Task AddExistingModuleThrows()
        {
            var loader = new ModuleLoader(_ => null!);

            loader.Add(typeof(object));

            var exception = Assert.Throws(() => loader.Add(typeof(object)));

            await Assert.That(exception).IsTypeOf<InvalidOperationException>();
        }

        [Test]
        public async Task AddedModuleIsVisibleInCatalog()
        {
            var loader = new ModuleLoader(_ => null!);

            loader.Add(typeof(object));

            await Assert.That(loader.ModuleTypes).Contains(typeof(object));
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
