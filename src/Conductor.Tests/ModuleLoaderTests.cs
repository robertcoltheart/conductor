using System;
using System.Linq;
using NUnit.Framework;

namespace Conductor
{
    [TestFixture]
    public class ModuleLoaderTests
    {
        [Test]
        public void NullConstructorThrows()
        {
            Assert.That(() => new ModuleLoader(null), Throws.ArgumentNullException);
        }

        [Test]
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

            Assert.That(type, Is.EqualTo(typeof(object)));
        }

        [Test]
        public void InitialiseCalledOnModule()
        {
            var module = new MockModule();

            var loader = new ModuleLoader(x => module);
            loader.Add(typeof (object));

            loader.Initialize();

            Assert.That(module.InitializeCalled, Is.True);
        }

        [Test]
        public void InnerErrorInInitialiseRaisesModuleInitialiseException()
        {
            var module = new InvalidModule();

            var loader = new ModuleLoader(x => module);
            loader.Add(typeof(object));

            Assert.That(() => loader.Initialize(), Throws.TypeOf<InitializeModuleException>().And.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void NullModuleReturnedThrows()
        {
            var loader = new ModuleLoader(x => null);
            loader.Add(typeof(object));

            Assert.That(() => loader.Initialize(), Throws.TypeOf<InitializeModuleException>());
        }

        [Test]
        public void InitializeAfterInitializedThrows()
        {
            var loader = new ModuleLoader(x => null);
            loader.Initialize();

            Assert.That(() => loader.Initialize(), Throws.InvalidOperationException);
        }

        [Test]
        public void CreatesWithEmptyModules()
        {
            var loader = new ModuleLoader(x => null);

            Assert.That(loader.ModuleTypes, Is.Not.Null);
            Assert.That(loader.ModuleTypes.Any(), Is.False);
        }

        [Test]
        public void NullAddArgumentThrows()
        {
            var loader = new ModuleLoader(x => null);

            Assert.That(() => loader.Add(null), Throws.ArgumentNullException);
        }

        [Test]
        public void AddExistingModuleThrows()
        {
            var loader = new ModuleLoader(x => null);

            loader.Add(typeof(object));

            Assert.That(() => loader.Add(typeof(object)), Throws.InvalidOperationException);
        }

        [Test]
        public void AddedModuleIsVisibleInCatalog()
        {
            var loader = new ModuleLoader(x => null);

            loader.Add(typeof(object));

            Assert.That(loader.ModuleTypes.Contains(typeof(object)), Is.True);
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
