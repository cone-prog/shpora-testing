
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void ConstructorShould_ReturnNewlyObject_WhenParametersIsValid()
    {
        var exptected = new NumberValidator(2,1, true);
        exptected.Should().NotBeNull();
    }
    
    [TestCase(-1, 2)]
    [TestCase(2, -2)]
    public void ConstructorShould_ThrowArgumentException_WhenParametersLessZero(int precision, int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, true));
    }

    [TestCase(2, 2)]
    [TestCase(2, 3)]
    public void ConstructorShould_ThrowArgumentException_WhenScaleEqualOrGreaterPrecision(int precision, int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, true));
    }

    [TestCase(17, 2, true, "0", true)]
    [TestCase(17, 2, true, "0.0", true)]
    [TestCase(3, 2, true, "00.00", false)]
    [TestCase(3, 2, true, "-0.00", false)]
    [TestCase(4, 2, true, "+1.23", true)]
    [TestCase(3, 2, true, "+1.23", false)]
    [TestCase(17, 2, true, "0.000", false)]
    [TestCase(3, 2, true, "-1.23", false)]
    [TestCase(3, 2, true, "a.sd", false)]
    [TestCase(3, 0, false, "1234", false)]
    [TestCase(3, 0, false, "-123", false)]
    [TestCase(4, 2, true, "-1.23", false)] 
    [TestCase(3, 0, true, "+12", true)]
    [TestCase(3, 2, false, "1.2.3", false)]
    [TestCase(3, 2, false, "--1", false)]
    [TestCase(3, 2, false, "", false)]
    [TestCase(3, 2, false, "   ", false)]
    [TestCase(3, 2, false, null, false)]
    public void IsValidNumberShould_ReturnsExpected(int precision, int scale, bool onlyPositive, string? value,
        bool expected) 
    { 
        var validator = new NumberValidator(precision, scale, onlyPositive);

        var actual = validator.IsValidNumber(value!); 

        Assert.That(actual, Is.EqualTo(expected)); 
    }    
}