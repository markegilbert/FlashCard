using NUnit.Framework;
using FlashCard.Configuration;

namespace UnitTests
{
    [TestFixture]
    public class CosmosDBSettingsTests
    {
        private CosmosDBSettings? _Settings;


        [SetUp]
        public void SetUp()
        {
            this._Settings = new CosmosDBSettings();
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void AccountEndpoint_AssignedSomethingInvalid_ReturnsEmpty(String TestValue)
        {
            this._Settings.AccountEndpoint = TestValue;
            Assert.That(this._Settings.AccountEndpoint, Is.Empty);
        }
        [Test]
        public void AccountEndpoint_AssignedSomethingValid_ReturnsThatValue()
        {
            this._Settings.AccountEndpoint = "http://localhost:8081";
            Assert.That(this._Settings.AccountEndpoint, Is.EqualTo("http://localhost:8081"));
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void DatabaseName_AssignedSomethingInvalid_ReturnsEmpty(String TestValue)
        {
            this._Settings.DatabaseName = TestValue;
            Assert.That(this._Settings.DatabaseName, Is.Empty);
        }
        [Test]
        public void DatabaseName_AssignedSomethingValid_ReturnsThatValue()
        {
            this._Settings.DatabaseName = "database_name";
            Assert.That(this._Settings.DatabaseName, Is.EqualTo("database_name"));
        }


        [Test]
        public void FlashCardContainer_IsNotNullOnInstantiation()
        {
            Assert.That(this._Settings.FlashCardContainer, Is.Not.Null, "Property should not have been null on instantiation");
        }
        [Test]
        public void TopicContainer_IsNotNullOnInstantiation()
        {
            Assert.That(this._Settings.TopicContainer, Is.Not.Null, "Property should not have been null on instantiation");
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void PrimaryKey_AssignedSomethingInvalid_ReturnsEmpty(String TestValue)
        {
            this._Settings.PrimaryKey = TestValue;
            Assert.That(this._Settings.PrimaryKey, Is.Empty);
        }
        [Test]
        public void PrimaryKey_AssignedSomethingValid_ReturnsThatValue()
        {
            this._Settings.PrimaryKey = "abc123";
            Assert.That(this._Settings.PrimaryKey, Is.EqualTo("abc123"));
        }


    }
}
