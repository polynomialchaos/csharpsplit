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

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class OrderdDictionary<TKey, TVal>
    : IDictionary<TKey, TVal> where TKey : notnull
{
    private Dictionary<TKey, TVal> dictionary;
    private List<TKey> dictionary_keys = new();

    public OrderdDictionary()
    {
        dictionary = new Dictionary<TKey, TVal>();
    }

    public void Add(TKey key, TVal value)
    {
        dictionary_keys.Add(key);
        dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TVal> item)
    {
        dictionary_keys.Add(item.Key);
        dictionary.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        dictionary.Clear();
    }

    public IEqualityComparer<TKey> Comparer
    {
        get { return dictionary.Comparer; }
    }

    public bool Contains(KeyValuePair<TKey, TVal> item)
    {
        TVal v;
        return (dictionary.TryGetValue(item.Key, out v) && v.Equals(item.Key));
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool ContainsValue(TVal value)
    {
        return dictionary.ContainsValue(value);
    }

    public int Count
    {
        get { return dictionary.Count; }
    }

    public void CopyTo(KeyValuePair<TKey, TVal>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TVal>>)dictionary)
            .CopyTo(array, arrayIndex);
    }

    public void CopyTo(Array array, int index)
    {
        ((ICollection)dictionary).CopyTo(array, index);
    }

    public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
    {
        foreach (TKey key in dictionary_keys)
        {
            yield return new KeyValuePair<TKey, TVal>(key, this[key]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool IsReadOnly { get; private set; }

    public ICollection<TKey> Keys
    {
        get { return dictionary_keys; }
    }

    public bool Remove(TKey key)
    {
        dictionary_keys.Remove(key);
        return dictionary.Remove(key);
    }
    public bool Remove(KeyValuePair<TKey, TVal> item)
    {
        if (Contains(item))
        {
            dictionary_keys.Remove(item.Key);
            dictionary.Remove(item.Key);
            return true;
        }
        return false;
    }

    public IEnumerable<R> Select<R>(Func<KeyValuePair<TKey, TVal>, R> selector)
    {
        List<R> results = new();
        foreach (KeyValuePair<TKey, TVal> item in this)
        {
            results.Add(selector(item));
        }

        return results;
    }

    public TVal this[TKey key]
    {
        get { return dictionary[key]; }
        set { dictionary_keys.Add(key); dictionary[key] = value; }
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TVal value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public override string ToString()
    {
        return "{" + String.Join(",",
            Select<string>(it => "\"" + it.Key + "\":" + it.Value).ToList()
        ) + "}";
    }

    public ICollection<TVal> Values
    {
        get
        {
            return Keys.ToList<TKey>().Select(it => this[it]).ToList();
        }
    }
}