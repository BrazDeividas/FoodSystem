using CustomMap;
using static CustomMap.MapEnums;

namespace Tests.MapValueToEnumTests;

internal class MapValueToEnum_Tests
{
    [Test]
    public void MapValueToEnum_IntToGender_ReturnsGenderEnum()
    {
        var result = MapValueToEnum<Gender>(1);

        Assert.That(result, Is.TypeOf(typeof(Gender)));
        Assert.That(result, Is.EqualTo(Gender.Male));
    }

    [Test]
    public void MapValueToEnum_StringToGender_ReturnsGenderEnum()
    {
        var result = MapValueToEnum<Gender>("Female");

        Assert.That(result, Is.TypeOf(typeof(Gender)));
        Assert.That(result, Is.EqualTo(Gender.Female));
    }

    [Test]
    public void MapValueToEnum_StringToWeekday_ReturnsWeekdayEnum()
    {
        var result = MapValueToEnum<Weekday>("Wednesday");

        Assert.That(result, Is.TypeOf(typeof(Weekday)));
        Assert.That(result, Is.EqualTo(Weekday.Wednesday));
    }

    [Test]
    public void MapValueToEnum_InvalidIntToGender_ReturnsException()
    {
        Action<int> callback = null;

        callback = (int value) => MapValueToEnum<Gender>(value);

        Assert.That(callback, Is.Not.Null);
        Assert.Throws<Exception>(() => callback(4));
    }

    [Test]
    public void MapValueToEnum_InvalidIntToWeekday_ReturnsException()
    {
        Func<int, Weekday> mapper = MapValueToEnum<Weekday>;

        Assert.Throws<Exception>(() => mapper(7));
    }

    [Test]
    public void MapValueToEnum_InvalidStringToWeekday_ReturnsException()
    {
        Func<string, Weekday> mapper = MapValueToEnum<Weekday>;

        Assert.Throws<Exception>(() => mapper("InvalidDay"));
    }

    public delegate Gender IntToGenderMapper(int value);
}
