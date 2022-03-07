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

/// <summary>Generic OrderdDictionary class which keeps insertion
/// order of items in mind.</summary>
public sealed class OrderdDictionary<TKey, TVal> where TKey : notnull
{
    private Dictionary<TKey, TVal> dictionary = new Dictionary<TKey, TVal>();
    private List<TKey> dictionary_keys = new();

    /// <summary>Initialize an OrderedDictionary object.</summary>
    public OrderdDictionary() { }

    /// <summary>Adds a key and value to the dictionary.</summary>
    public void Add(TKey key, TVal value)
    {
        dictionary_keys.Add(key);
        dictionary.Add(key, value);
    }

    /// <summary>Adds a KeyValuePair to the dictionary.</summary>
    public void Add(KeyValuePair<TKey, TVal> item)
    {
        dictionary_keys.Add(item.Key);
        dictionary.Add(item.Key, item.Value);
    }

    /// <summary>Clears the dictionary.</summary>
    public void Clear()
    {
        dictionary_keys.Clear();
        dictionary.Clear();
    }

    /// <summary>Checks if the KeyValuePair is in the dictionary.</summary>
    /// <returns>A boolean flag.</returns>
    public bool Contains(KeyValuePair<TKey, TVal> item)
    {
        TVal? v;
        if (dictionary.TryGetValue(item.Key, out v))
        {
            return v == null ? false : v.Equals(item.Value);
        }

        return false;
    }

    /// <summary>Checks if the Key is in the dictionary.</summary>
    /// <returns>A boolean flag.</returns>
    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    /// <summary>Checks if the Value is in the dictionary</summary>
    /// <returns>A boolean flag.</returns>
    public bool ContainsValue(TVal value)
    {
        return dictionary.ContainsValue(value);
    }

    /// <summary>Gets the number of contained KeyValuePairs.</summary>
    /// <returns>A integer value.</returns>
    public int Count
    {
        get { return dictionary.Count; }
    }

    /// <summary>Gets the contained KeyValuePairs.</summary>
    /// <returns>An KeyValuePair IEnumerator of type TKey and TVal.</returns>
    public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
    {
        foreach (TKey key in dictionary_keys)
        {
            yield return new KeyValuePair<TKey, TVal>(key, this[key]);
        }
    }

    /// <summary>Gets the dictionary keys.</summary>
    /// <returns>A ICollection of type TKey.</returns>
    public ICollection<TKey> Keys
    {
        get { return dictionary_keys; }
    }

    /// <summary>Removes a key from the dictionary.</summary>
    /// <returns>A boolean flag.</returns>
    public bool Remove(TKey key)
    {
        bool result = dictionary.Remove(key);
        if (result)
            dictionary_keys.Remove(key);

        return result;
    }

    /// <summary>Removes a KeyValuePair from the dictionary.</summary>
    /// <returns>A boolean flag.</returns>
    public bool Remove(KeyValuePair<TKey, TVal> item)
    {
        if (Contains(item))
            return Remove(item.Key);

        return false;
    }

    /// <summary>Gets the enumerator over all Enumeration items.</summary>
    /// <returns>The IEnumerable converted by the selector.</returns>
    public IEnumerable<R> Select<R>(Func<KeyValuePair<TKey, TVal>, R> selector)
    {
        foreach (KeyValuePair<TKey, TVal> item in this)
        {
            yield return selector(item);
        }
    }

    /// <summary>Gets/sets the dictionary value by key.</summary>
    /// <returns>A TVal object.</returns>
    public TVal this[TKey key]
    {
        get { return dictionary[key]; }
        set
        {
            if (!ContainsKey(key))
                dictionary_keys.Add(key);

            dictionary[key] = value;
        }
    }

    /// <summary>Converts to an equivalent string.</summary>
    /// <returns>A string.</returns>
    public override string ToString()
    {
        return String.Format("{{{0}}}", String.Join(",",
            Select<string>(it => "\"" + it.Key + "\":" + it.Value).ToList()
        ));
    }

    /// <summary>Gets the dictionary keys.</summary>
    /// <returns>A ICollection of type TVal.</returns>
    public ICollection<TVal> Values
    {
        get
        {
            return Keys.Select(it => this[it]).ToList();
        }
    }
}