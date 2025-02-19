using NUnit.Framework;
using FlashCard.Database;
using Castle.Core.Logging;
using NSubstitute;
using FlashCard.Models;


namespace UnitTests
{
    public class FlashCardsRepositoryTests
    {
        private ArgumentNullException _ArgumentNullException;
        private FlashCardsRepository _Repository;
        private FlashCardDBContext _ContextMock;
        private List<OrderByItem> _ActualList;

        [SetUp]
        public void SetUp()
        {
            this._ArgumentNullException = null;
            this._ContextMock = Substitute.For<FlashCardDBContext>("https://localhost", "abc123", "SomeDatabaseName", "SomeContainerName");
            this._Repository = new FlashCardsRepository(this._ContextMock);
            this._ActualList = null;
        }


        [Test]
        public void InstantiateClass_ContextIsNull_ExceptionThrown()
        {
            this._ArgumentNullException = Assert.Throws<ArgumentNullException>(() => new FlashCardsRepository(null));
            Assert.That(this._ArgumentNullException.Message.Contains("'Context'"), "Exception didn't reference the parameter in error");
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void ToOrderByFuncList_SortStringIsNotValid_EmptyListReturned(String TestValue)
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>(TestValue);

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList, Is.Empty, "List should not been empty");
        }
        [Test]
        public void ToOrderByFuncList_SortStringHasSingleValidProperty_ListWithThatReturned_SortedAsc()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("CreatedOn");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count() == 1, "List had the wrong number of entries");
            Assert.That(this._ActualList[0].Ascending, Is.True, "Sort[0] should have been ascending");
        }

    }
}
