namespace Conductor.Tests
{
    public class InitializeModuleExceptionTests
    {
        [Test]
        public async Task IsTypeOfException()
        {
            var exception = new InitializeModuleException();

            await Assert.That(exception).IsAssignableTo<Exception>();
        }

        [Test]
        public async Task InnerExceptionSet()
        {
            var innerException = new Exception();
            var exception = new InitializeModuleException("msg", innerException);

            await Assert.That(innerException).IsSameReferenceAs(exception.InnerException);
        }

        [Test]
        public async Task HasMessage()
        {
            var exception = new InitializeModuleException("msg");

            await Assert.That(exception.Message).IsEqualTo("msg");
        }
    }
}
