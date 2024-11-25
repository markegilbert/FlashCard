using NUnit.Framework;
using FlashCard.Controllers;
using NSubstitute;
using Microsoft.Extensions.Logging;
using FlashCard.Database;


namespace UnitTests
{
    [TestFixture]
    public class TopicsControllerTests
    {
        private ILogger<TopicsController> _Logger;
        private TopicDBContext _DBContext;
        private ArgumentNullException? _ArgumentNullException;


        [SetUp]
        public void SetUp()
        {
            this._Logger = Substitute.For<ILogger<TopicsController>>();
            this._DBContext = Substitute.For<TopicDBContext>("https://localhost", "abc123", "SomeDatabaseName", "SomeContainerName");

            this._ArgumentNullException = null;
        }


        [Test]
        public void InstantiateClass_LoggerIsNull_ExceptionThrown()
        {
            this._ArgumentNullException = Assert.Throws<ArgumentNullException>(() => new TopicsController(null, this._DBContext));
            Assert.That(this._ArgumentNullException.Message.Contains($"'Logger'"), "Exception didn't reference the parameter in error");
        }
        [Test]
        public void InstantiateClass_DBContextIsNull_ExceptionThrown()
        {
            this._ArgumentNullException = Assert.Throws<ArgumentNullException>(() => new TopicsController(this._Logger, null));
            Assert.That(this._ArgumentNullException.Message.Contains($"'Context'"), "Exception didn't reference the parameter in error");
        }

    }
}
