using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Conductor
{
    [TestFixture]
    public class InitializeModuleExceptionTests
    {
        [Test]
        public void IsTypeOfException()
        {
            var exception = new InitializeModuleException();

            Assert.That(exception, Is.AssignableTo<Exception>(), "Type is not an exception.");
        }

        [Test]
        public void InnerExceptionSet()
        {
            var innerException = new Exception();
            var exception = new InitializeModuleException("msg", innerException);

            Assert.That(exception.InnerException, Is.SameAs(innerException));
        }

        [Test]
        public void HasMessage()
        {
            var exception = new InitializeModuleException("msg");

            Assert.That(exception.Message, Is.EqualTo("msg"));
        }

        [Test]
        public void IsSerializableDecorated()
        {
            Assert.That(typeof(InitializeModuleException).GetCustomAttributes(typeof(SerializableAttribute), true), Has.Length.EqualTo(1), "Type must be serializable.");
        }

        [Test]
        public void IsSerializable()
        {
            var exception = new InitializeModuleException();

            try
            {
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, exception);

                    stream.Position = 0;

                    exception = (InitializeModuleException)formatter.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                Assert.Fail("Type must be serializable.");
            }

            Assert.That(exception, Is.Not.Null, "Type could not be deserialized.");
        }
    }
}
