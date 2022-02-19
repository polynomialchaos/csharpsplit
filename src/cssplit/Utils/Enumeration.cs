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

    public static V FromName<V>(T name) where V : Enumeration<T, U>, new()
    {
        var matchingItem = Parse<V, T>(name, "name", item => item.name.Equals(name));
        return matchingItem;
    }


    public static V FromValue<V>(U value) where V : Enumeration<T, U>, new()
    {
        var matchingItem = Parse<V, U>(value, "value", item => item.value.Equals(value));
        return matchingItem;
    }

    public static IEnumerable<V> GetAll<V>() where V : Enumeration<T, U>, new()
    {
        var type = typeof(V);
        FieldInfo[] fields = type.GetFields(
            BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly
        );

        foreach (var info in fields)
        {
            var instance = new V();
            var locatedValue = info.GetValue(instance) as V;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    private static V Parse<V, W>(W value, string description, Func<V, bool> predicate) where V : Enumeration<T, U>, new()
    {
        var matchingItem = GetAll<V>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = string.Format("'{0}' is not a valvalue {1} in {2}", value, description, typeof(V));
            throw new ApplicationException(message);
        }

        return matchingItem;
    }

    public override string? ToString()
    {
        return name.ToString();
    }
}