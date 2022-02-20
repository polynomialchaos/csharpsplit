using System.Reflection;

public abstract class Enumeration<T, U> : IComparable
    where T : IComparable<T>
    where U : IComparable<U>
{
    public T name { get; private set; }
    public U value { get; private set; }

    protected Enumeration(T name, U value)
    {
        this.name = name;
        this.value = value;
    }

    public int CompareTo(object? other)
    {
        return other == null ? 1 : value.CompareTo(((Enumeration<T, U>)other).value);
    }

    public static V FromName<V>(T name) where V : Enumeration<T, U>
    {
        return Parse<V, T>(name, "name", item => item.name.Equals(name));
    }

    public static V FromValue<V>(U value) where V : Enumeration<T, U>
    {
        return Parse<V, U>(value, "value", item => item.value.Equals(value));
    }

    public static IEnumerable<V> GetAll<V>() where V : Enumeration<T, U>
    {
        Type type = typeof(V);
        FieldInfo[] fields = type.GetFields(
            BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly
        );

        foreach (FieldInfo info in fields)
        {
            var locatedValue = info.GetValue(null) as V;
            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    private static V Parse<V, W>(W value, string description, Func<V, bool> predicate) where V : Enumeration<T, U>
    {
        var matchingItem = GetAll<V>().FirstOrDefault(predicate);
        if (matchingItem == null)
        {
            throw new ApplicationException(String.Format(
                "'{0}' is not a value {1} in {2}", value, description, typeof(V)
            ));
        }

        return matchingItem;
    }

    public override string? ToString()
    {
        return value.ToString();
    }
}