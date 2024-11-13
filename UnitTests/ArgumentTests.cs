﻿using NUnit.Framework;
using DataImporter;
using System;


namespace UnitTests
{
    [TestFixture]
    public class ArgumentTests
    {
        private ArgumentException? _ArgumentException;
        private String[] _Args;
        private String? _ActualValue;


        [SetUp]
        public void SetUp()
        {
            this._ArgumentException = null;
            this._ActualValue = null;
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void ExtractValueFor_ArgumentNameIsInvalid_ExceptionThrown(String TestValue)
        {
            this._Args = new String[0];

            this._ArgumentException = Assert.Throws<ArgumentException>(() => this._Args.ExtractValueFor(TestValue));
            Assert.That(this._ArgumentException is not null && this._ArgumentException.Message.Contains("'ArgumentName'"), "Exception should have mentioned the parameter in error");
        }
        [Test]
        public void ExtractValueFor_ArgsIsEmpty_EmptyStringReturnedForArgumentRequested()
        {
            this._Args = new String[0];

            this._ActualValue = this._Args.ExtractValueFor("test1");

            Assert.That(this._ActualValue, Is.Empty);
        }
        [Test]
        public void ExtractValueFor_ArgsContainsTheArgumentNameButNoValueAfter_EmptyStringReturned()
        {
            this._Args = new String[1] { "--test1" };

            this._ActualValue = this._Args.ExtractValueFor("test1");

            Assert.That(this._ActualValue, Is.Empty);
        }
        [Test]
        public void ExtractValueFor_ArgsContainsArgumentAndValue_ValueIsReturned()
        {
            this._Args = new String[2] { "--test1", "value1" };

            this._ActualValue = this._Args.ExtractValueFor("test1");

            Assert.That(this._ActualValue, Is.EqualTo("value1"));
        }
        [Test]
        public void ExtractValueFor_ArgsContainsArgumentNameButMissingTheDashes_EmptyStringReturned()
        {
            this._Args = new String[2] { "test1", "value1" };

            this._ActualValue = this._Args.ExtractValueFor("test1");

            Assert.That(this._ActualValue, Is.Empty);
        }
        [Test]
        public void ExtractValueFor_ArgumentNameDoesntMatchInCase_DifferencesInCaseAreIgnoredAndValueIsReturned()
        {
            this._Args = new String[2] { "--TeSt1", "value1" };

            this._ActualValue = this._Args.ExtractValueFor("test1");

            Assert.That(this._ActualValue, Is.EqualTo("value1"));
        }
        [Test]
        public void ExtractValueFor_MultipleArgsPassedIn_CorrectValueIsExtracted()
        {
            this._Args = new String[6] { "--test1", "value1", "--test2", "value2", "--test3", "value3" };

            this._ActualValue = this._Args.ExtractValueFor("test2");

            Assert.That(this._ActualValue, Is.EqualTo("value2"));
        }
        [Test]
        public void ExtractValueFor_MultipleArgsPassedIn_MiddleOneIsMissingValue_LastParameterIsRequestedAndValueIsReturnedProperly()
        {
            this._Args = new String[5] { "--test1", "value1", "--test2", "--test3", "value3" };

            this._ActualValue = this._Args.ExtractValueFor("test3");

            Assert.That(this._ActualValue, Is.EqualTo("value3"));
        }
        [Test]
        public void ExtractValueFor_MultipleArgsPassedIn_LastOneIsMissingValue_LastParameterIsRequestedAndEmptyStringReturned()
        {
            this._Args = new String[5] { "--test1", "value1", "--test2", "value2", "--test3" };

            this._ActualValue = this._Args.ExtractValueFor("test3");

            Assert.That(this._ActualValue, Is.Empty);
        }

    }
}
