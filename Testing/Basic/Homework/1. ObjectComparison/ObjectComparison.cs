using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Перепишите код на использование Fluent Assertions.
        
        // Этот вариант лучше по следующим причинам:
        // - Теперь нет зависимости от кол-во полей: BeEquivalentTo - сравнивает все поля объекта(в другом тесте
        // приходится добавлять поля в методе AreEqual(). Также исключили сранение Id на всех уровнях сложности.
        // - Также мы теперь не зависим от вложенности (насколько далеко по полю parent мы можем пройти), и к тому же
        // AllowingInfiniteRecursion() убирает ограничение глубины рекурсии.
        // - Еще мы видим какие именно поля разные
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.AllowingInfiniteRecursion()
                   .Excluding((IMemberInfo p) => p.Name == "Id"));
        
        // ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        // ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        // ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        // ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);
        //
        // ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
