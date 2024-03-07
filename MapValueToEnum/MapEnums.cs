namespace CustomMap;

public static class MapEnums
{
    public enum Gender : int
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum Weekday
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public static void Main()
    {
    }

    public static T MapValueToEnum<T>(string value) where T : struct, IConvertible
    {
        object? result;

        if (!Enum.TryParse(typeof(T), value, out result) || !Enum.IsDefined(typeof(T), result))
        {
            throw new Exception($"Value '{value}' is not part of a valid Enum");
        }

        return (T)result;
    }

    public static T MapValueToEnum<T>(int value) where T : struct, IConvertible
    {
        object? result;
        

        if (!Enum.TryParse(typeof(T), value.ToString(), out result) || !Enum.IsDefined(typeof(T), result))
        {
            throw new Exception($"Value '{value}' is not part of a valid Enum");
        }

        return (T)result;
    }
}