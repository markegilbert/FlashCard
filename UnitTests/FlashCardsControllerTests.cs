using FlashCard.Controllers;
using FlashCard.Database;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;


namespace UnitTests
{
    public class FlashCardsControllerTests
    {
        private ILogger<FlashCardsController> _Logger;
        private FlashCardsRepository _Repository;
        private ArgumentNullException? _ArgumentNullException;


        [SetUp]
        public void SetUp()
        {
            this._Logger = Substitute.For<ILogger<FlashCardsController>>();
            this._Repository = Substitute.For<FlashCardsRepository>(new FlashCardDBContext("", "", "", ""));

            this._ArgumentNullException = null;
        }

        [Test]
        public void InstantiateClass_LoggerIsNull_ExceptionThrown()
        {
            this._ArgumentNullException = Assert.Throws<ArgumentNullException>(() => new FlashCardsController(null, this._Repository));
            Assert.That(this._ArgumentNullException.Message.Contains($"'Logger'"), "Exception didn't reference the parameter in error");
        }
        [Test]
        public void InstantiateClass_DBContextIsNull_ExceptionThrown()
        {
            this._ArgumentNullException = Assert.Throws<ArgumentNullException>(() => new FlashCardsController(this._Logger, null));
            Assert.That(this._ArgumentNullException.Message.Contains($"'Repository'"), "Exception didn't reference the parameter in error");
        }

    }
}
