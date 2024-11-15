using NUnit.Framework;
using FlashCard.Configuration;


namespace UnitTests
{
    public class ContainerSettingsTests
    {
        private ContainerSettings _Settings;

        [SetUp]
        public void SetUp()
        {
            this._Settings = new ContainerSettings();
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
        public void PartitionKeyPath_AssignedSomethingInvalid_ReturnsEmpty(String TestValue)
        {
            this._Settings.PartitionKeyPath = TestValue;
            Assert.That(this._Settings.PartitionKeyPath, Is.Empty);
        }
        [Test]
        public void ContainerPartitionKeyPath_AssignedSomethingValid_ReturnsThatValue()
        {
            this._Settings.PartitionKeyPath = "/id";
            Assert.That(this._Settings.PartitionKeyPath, Is.EqualTo("/id"));
        }
    }
}
