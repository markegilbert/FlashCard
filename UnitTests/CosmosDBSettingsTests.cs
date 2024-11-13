using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DataImporter;

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


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void ContainerID_AssignedSomethingInvalid_ReturnsEmpty(String TestValue)
        {
            this._Settings.ContainerID = TestValue;
            Assert.That(this._Settings.ContainerID, Is.Empty);
        }
        [Test]
        public void ContainerID_AssignedSomethingValid_ReturnsThatValue()
        {
            this._Settings.ContainerID = "sometable";
            Assert.That(this._Settings.ContainerID, Is.EqualTo("sometable"));
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void ContainerPartitionKeyPath_AssignedSomethingInvalid_ReturnsEmpty(String TestValue)
        {
            this._Settings.ContainerPartitionKeyPath = TestValue;
            Assert.That(this._Settings.ContainerPartitionKeyPath, Is.Empty);
        }
        [Test]
        public void ContainerPartitionKeyPath_AssignedSomethingValid_ReturnsThatValue()
        {
            this._Settings.ContainerPartitionKeyPath = "/id";
            Assert.That(this._Settings.ContainerPartitionKeyPath, Is.EqualTo("/id"));
        }

    }
}
