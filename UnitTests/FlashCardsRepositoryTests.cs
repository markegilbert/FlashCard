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
        public void ToOrderByFuncList_RawOrderByIsNotValid_EmptyListReturned(String TestValue)
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>(TestValue);

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList, Is.Empty, "List should not been empty");
        }
        [Test]
        public void ToOrderByFuncList_SingleValidProperty_ListWithThatReturned_SortedAscending()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("CreatedOn");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(1), "List had the wrong number of entries");
            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.True, "Sort[0] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_SingleInvalidProperty_EmptyListReturned()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("BlahBlah");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(0), "List had the wrong number of entries");
        }
        [Test]
        public void ToOrderByFuncList_ValueHasLeadingPlus_ItemIsSortedAscending()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("+CreatedOn");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(1), "List had the wrong number of entries");
            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.True, "Sort[0] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_ValueHasHasLeadingMinus_ItemIsSortedDescending()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("-CreatedOn");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(1), "List had the wrong number of entries");
            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.False, "Sort[0] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_MultipleValidProperties_ListWithThoseReturned_AllSortedAscending()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("CreatedOn,Question,Answer");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(3), "List had the wrong number of entries");

            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.True, "Sort[0] sort direction was not correct");

            Assert.That(this._ActualList[1].SortFunction.Name, Is.EqualTo("Question"), "Sort[1] name was incorrect");
            Assert.That(this._ActualList[1].Ascending, Is.True, "Sort[1] sort direction was not correct");

            Assert.That(this._ActualList[2].SortFunction.Name, Is.EqualTo("Answer"), "Sort[2] name was incorrect");
            Assert.That(this._ActualList[2].Ascending, Is.True, "Sort[2] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_MultipleValidProperties_AllWithLeadingPlusses_AllSortedAscending()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("+CreatedOn,+Question,+Answer");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(3), "List had the wrong number of entries");

            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.True, "Sort[0] sort direction was not correct");

            Assert.That(this._ActualList[1].SortFunction.Name, Is.EqualTo("Question"), "Sort[1] name was incorrect");
            Assert.That(this._ActualList[1].Ascending, Is.True, "Sort[1] sort direction was not correct");

            Assert.That(this._ActualList[2].SortFunction.Name, Is.EqualTo("Answer"), "Sort[2] name was incorrect");
            Assert.That(this._ActualList[2].Ascending, Is.True, "Sort[2] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_MultipleValidProperties_AllWithLeadingMinusses_AllSortedDescending()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("-CreatedOn,-Question,-Answer");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(3), "List had the wrong number of entries");

            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.False, "Sort[0] sort direction was not correct");

            Assert.That(this._ActualList[1].SortFunction.Name, Is.EqualTo("Question"), "Sort[1] name was incorrect");
            Assert.That(this._ActualList[1].Ascending, Is.False, "Sort[1] sort direction was not correct");

            Assert.That(this._ActualList[2].SortFunction.Name, Is.EqualTo("Answer"), "Sort[2] name was incorrect");
            Assert.That(this._ActualList[2].Ascending, Is.False, "Sort[2] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_MultipleValidProperties_MixOfPrefixes_SortOrdersAreAllCorrect()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>("-CreatedOn,Question,+Answer");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(3), "List had the wrong number of entries");

            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.False, "Sort[0] sort direction was not correct");

            Assert.That(this._ActualList[1].SortFunction.Name, Is.EqualTo("Question"), "Sort[1] name was incorrect");
            Assert.That(this._ActualList[1].Ascending, Is.True, "Sort[1] sort direction was not correct");

            Assert.That(this._ActualList[2].SortFunction.Name, Is.EqualTo("Answer"), "Sort[2] name was incorrect");
            Assert.That(this._ActualList[2].Ascending, Is.True, "Sort[2] sort direction was not correct");
        }
        [Test]
        public void ToOrderByFuncList_MultipleValidProperties_ExtraSpacesIncluded_ExtraSpaceIgnored()
        {
            this._ActualList = this._Repository.ToOrderByFuncList<FlashCardModel>(" CreatedOn , -Question , Answer ");

            Assert.That(this._ActualList, Is.Not.Null, "List should not have been null");
            Assert.That(this._ActualList.Count(), Is.EqualTo(3), "List had the wrong number of entries");

            Assert.That(this._ActualList[0].SortFunction.Name, Is.EqualTo("CreatedOn"), "Sort[0] name was incorrect");
            Assert.That(this._ActualList[0].Ascending, Is.True, "Sort[0] sort direction was not correct");

            Assert.That(this._ActualList[1].SortFunction.Name, Is.EqualTo("Question"), "Sort[1] name was incorrect");
            Assert.That(this._ActualList[1].Ascending, Is.False, "Sort[1] sort direction was not correct");

            Assert.That(this._ActualList[2].SortFunction.Name, Is.EqualTo("Answer"), "Sort[2] name was incorrect");
            Assert.That(this._ActualList[2].Ascending, Is.True, "Sort[2] sort direction was not correct");
        }

    }
}
