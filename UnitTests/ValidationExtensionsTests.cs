using NUnit.Framework;
using FlashCard.Configuration;
using System.ComponentModel.DataAnnotations;


namespace UnitTests
{
    [TestFixture]
    public class ValidationExtensionsTests
    {
        private Exception _Exception;

        [SetUp]
        public void SetUp()
        {
            this._Exception = null;
        }

        [Test]
        public void ValidateDataAnnotations_ClassHasNoValidationAttributes_NoExceptionThrown()
        {
            ClassWithNoValidations ObjectUnderTest = new ClassWithNoValidations();

            Assert.DoesNotThrow(() => ValidationExtensions.ValidateDataAnnotations(ObjectUnderTest));
        }
        [Test]
        public void ValidateDataAnnotations_ClassHasValidationAttributes_ObjectPropertiesNotSet_ExceptionThrown()
        {
            ClassWithValidations ObjectUnderTest = new ClassWithValidations();

            this._Exception = Assert.Throws<Exception>(() => ValidationExtensions.ValidateDataAnnotations(ObjectUnderTest));
            Assert.That(this._Exception.Message.Contains("StringProperty field"), "Exception should have mentioned the StringProperty property as being invalid");
            Assert.That(!this._Exception.Message.Contains("IntProperty field"), "Exception should NOT have mentioned the IntProperty property as being invalid");
            Assert.That(!this._Exception.Message.Contains("DoubleProperty field"), "Exception should NOT have mentioned the DoubleProperty property as being invalid");
        }
        [Test]
        public void ValidateDataAnnotations_ClassHasValidationAttributes_RequiredPropertiesAreSet_NoExceptionThrown()
        {
            ClassWithValidations ObjectUnderTest = new ClassWithValidations();
            ObjectUnderTest.StringProperty = "blah";

            Assert.DoesNotThrow(() => ValidationExtensions.ValidateDataAnnotations(ObjectUnderTest));
        }
    }


    public class ClassWithNoValidations
    { 
        public String? StringProperty { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
    }
    public class ClassWithValidations
    {
        [Required]
        public String? StringProperty { get; set; }
        [Required]
        public int IntProperty { get; set; }
        [Required]
        public double DoubleProperty { get; set; }
    }

}
