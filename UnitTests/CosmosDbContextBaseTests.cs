using NUnit.Framework;
using FlashCard.Database;


namespace UnitTests
{
    [TestFixture]
    public class CosmosDbContextBaseTests
    {
        private CosmosDbContextBase? _DbContext;
        private ArgumentException _ArgumentException;


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void InstantiateClass_AccountEndpointIsNotValid_ExceptionThrown(String TestValue)
        {
            this._ArgumentException = Assert.Throws<ArgumentException>(() => new CosmosDbContextBase(TestValue, "abc123", "SomeDatabase", "SomeContainer"));
            Assert.That(this._ArgumentException is not null && this._ArgumentException.Message.Contains("'AccountEndpoint'"), "Exception didn't reference the correct parameter");
        }
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void InstantiateClass_AuthorizationKeyIsNotValid_ExceptionThrown(String TestValue)
        {
            this._ArgumentException = Assert.Throws<ArgumentException>(() => new CosmosDbContextBase("auth_abc123", TestValue, "SomeDatabase", "SomeContainer"));
            Assert.That(this._ArgumentException is not null && this._ArgumentException.Message.Contains("'AuthorizationKey'"), "Exception didn't reference the correct parameter");
        }
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void InstantiateClass_DatabaseNameIsNotValid_ExceptionThrown(String TestValue)
        {
            this._ArgumentException = Assert.Throws<ArgumentException>(() => new CosmosDbContextBase("auth_abc123", "abc123", TestValue, "SomeContainer"));
            Assert.That(this._ArgumentException is not null && this._ArgumentException.Message.Contains("'DatabaseName'"), "Exception didn't reference the correct parameter");
        }
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void InstantiateClass_ContainerNameIsNotValid_ExceptionThrown(String TestValue)
        {
            this._ArgumentException = Assert.Throws<ArgumentException>(() => new CosmosDbContextBase("auth_abc123", "abc123", "SomeDatabase", TestValue));
            Assert.That(this._ArgumentException is not null && this._ArgumentException.Message.Contains("'ContainerName'"), "Exception didn't reference the correct parameter");
        }

    }
}
