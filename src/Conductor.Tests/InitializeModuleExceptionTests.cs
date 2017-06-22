using System;
using Xunit;

namespace Conductor
{
    public class InitializeModuleExceptionTests
    {
        [Fact]
        public void IsTypeOfException()
        {
            var exception = new InitializeModuleException();

            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void InnerExceptionSet()
        {
            var innerException = new Exception();
            var exception = new InitializeModuleException("msg", innerException);

            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void HasMessage()
        {
            var exception = new InitializeModuleException("msg");

            Assert.Equal("msg", exception.Message);
        }
    }
}
