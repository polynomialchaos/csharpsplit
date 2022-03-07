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

/// <summary>Base class defining required package class methods.</summary>
public class Base
{
    public TimeStamp time_stamp { get; private set; }

    /// <summary>Initialize a Base object.</summary>
    public Base()
    {
        time_stamp = new();
    }

    /// <summary>Serializes the object.</summary>
    /// <returns>A Dictionary of type string and object.</returns>
    protected virtual Dictionary<string, object> Serialize()
    {
        Dictionary<string, object> hash_map = new();
        return hash_map;
    }

    /// <summary>Sets the time from a DateTime object.</summary>
    public void SetTime(DateTime time)
    {
        time_stamp.SetTime(time);
    }

    /// <summary>Sets the time from a string.</summary>
    public void SetTime(string time_string)
    {
        time_stamp.SetTime(time_string);
    }

    /// <summary>Converts to an equivalent dictionary.</summary>
    /// <returns>A Dictionary of type string and object.</returns>
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> tmp = Serialize();
        tmp.Add("stamp", time_stamp.ToString());
        return tmp;
    }

    /// <summary>Converts to an equivalent string.</summary>
    /// <returns>A string.</returns>
    public override string ToString()
    {
        return String.Format(
            "<{0} stamp={1}>", base.ToString(),
            time_stamp.ToString());
    }
}