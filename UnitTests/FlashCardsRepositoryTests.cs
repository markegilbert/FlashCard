using NUnit.Framework;
using FlashCard.Database;


namespace UnitTests
{
    public class FlashCardsRepositoryTests
    {
        private ArgumentNullException _ArgumentNullException;

        [SetUp]
        public void SetUp()
        {
            this._ArgumentNullException = null;
        }


        [Test]
        public void InstantiateClass_ContextIsNull_ExceptionThrown()
        {
            this._ArgumentNullException = Assert.Throws<ArgumentNullException>(() => new FlashCardsRepository(null));
            Assert.That(this._ArgumentNullException.Message.Contains("'Context'"), "Exception didn't reference the parameter in error");
        }

    }
}
