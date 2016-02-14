using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Conductor
{
    [TestClass]
    public class InitializeModuleExceptionTests
    {
        [TestMethod]
        public void IsTypeOfException()
        {
            Assert.IsTrue(typeof(Exception).IsAssignableFrom(typeof(InitializeModuleException)), "Type is not an exception.");
        }

        [TestMethod]
        public void InnerExceptionSet()
        {
            var innerException = new Exception();
            var exception = new InitializeModuleException("msg", innerException);

            Assert.AreSame(innerException, exception.InnerException);
        }

        [TestMethod]
        public void HasMessage()
        {
            var exception = new InitializeModuleException("msg");

            Assert.AreEqual("msg", exception.Message);
        }

        [TestMethod]
        public void IsSerializableDecorated()
        {
            Assert.IsTrue(typeof(InitializeModuleException).GetCustomAttributes(typeof(SerializableAttribute), true).Length == 1, "Type must be serializable.");
        }

        [TestMethod]
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

            Assert.IsNotNull(exception, "Type could not be deserialized.");
        }
    }
}