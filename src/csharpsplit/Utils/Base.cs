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

public class Base
{
    public Stamp time { get; private set; }

    public Base()
    {
        time = new();
    }

    public void SetTime(DateTime time)
    {
        this.time.SetTime(time);
    }

    public void SetTime(string time_string)
    {
        this.time.SetTime(time_string);
    }

    public override string ToString()
    {
        return String.Format(
            "<{0} stamp={1}>", base.ToString(), time.ToString());
    }

    protected virtual OrderdDictionary<string, object> Serialize()
    {
        OrderdDictionary<string, object> hash_map = new();
        return hash_map;
    }

    public OrderdDictionary<string, object> ToDictionary()
    {
        OrderdDictionary<string, object> tmp = Serialize();
        tmp.Add("stamp", time.ToString());
        return tmp;
    }

    public static string ToDebugString(
        OrderdDictionary<string, object> dictionary)
    {
        return "{" + String.Join(",",
            dictionary.Select(
                it => it.Key + "=" + it.Value).ToArray()) + "}";
    }
}