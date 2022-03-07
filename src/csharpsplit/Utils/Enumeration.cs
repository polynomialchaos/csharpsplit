// MIT License

// Copyright (c) 2022 Florian Eigentler

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace CSharpSplit.Utils;

using System.Reflection;

/// <summary>Generic Enumeration class.</summary>
public abstract class Enumeration<TKey, TVal> : IComparable
    where TKey : IComparable<TKey>
    where TVal : IComparable<TVal>
{
    public TKey key { get; private set; }
    public TVal value { get; private set; }

    /// <summary>Initialize an Enumeration object with key and value.</summary>
    protected Enumeration(TKey key, TVal value)
    {
        this.key = key;
        this.value = value;
    }

    /// <summary>Compares the object to another object.</summary>
    /// <returns>A integer value.</returns>
    public int CompareTo(object? other)
    {
        return other == null ? 1 : value.CompareTo(
            ((Enumeration<TKey, TVal>)other).value);
    }

    /// <summary>Gets the enumeration from a key.</summary>
    /// <returns>An Enumeration item of type TKey and TVal.</returns>
    public static TEnum FromName<TEnum>(TKey key)
        where TEnum : Enumeration<TKey, TVal>
    {
        return Parse<TEnum, TKey>(key, "key", item => item.key.Equals(key));
    }

    /// <summary>Gets the enumeration from a value.</summary>
    /// <returns>An Enumeration item of type TKey and TVal.</returns>
    public static TEnum FromValue<TEnum>(TVal value)
        where TEnum : Enumeration<TKey, TVal>
    {
        return Parse<TEnum, TVal>(value, "value", item => item.value.Equals(value));
    }

    /// <summary>Gets the enumerator over all Enumeration items.</summary>
    /// <returns>An Enumeration IEnumerable of type TKey and TVal.</returns>
    public static IEnumerable<TEnum> GetAll<TEnum>()
        where TEnum : Enumeration<TKey, TVal>
    {
        Type type = typeof(TEnum);
        FieldInfo[] fields = type.GetFields(
            BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.DeclaredOnly
        );

        foreach (FieldInfo info in fields)
        {
            var locatedValue = info.GetValue(null) as TEnum;
            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    /// <summary>Gets the first Enumeration item where the defined
    /// predicate matches the item.</summary>
    /// <returns>An Enumeration of type TKey and TVal.</returns>
    private static TEnum Parse<TEnum, T>(T value, string description,
        Func<TEnum, bool> predicate) where TEnum : Enumeration<TKey, TVal>
    {
        var matchingItem = GetAll<TEnum>().FirstOrDefault(predicate);
        if (matchingItem == null)
        {
            throw new ApplicationException(String.Format(
                "'{0}' is not a value {1} in {2}", value, description, typeof(TEnum)
            ));
        }

        return matchingItem;
    }

    /// <summary>Converts the object to its equivalent string.</summary>
    /// <returns>A string.</returns>
    public override string? ToString()
    {
        return value.ToString();
    }
}