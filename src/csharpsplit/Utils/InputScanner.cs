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

public class InputScanner
{
    public InputScanner() { }

    public static T Get<T>(string description, Func<string, T> functor)
    {
        return Get(description, null, null, functor);
    }

    public static T Get<T>(string description,
        string default_value, Func<string, T> functor)
    {
        return Get(description, default_value, null, functor);
    }

    public static T Get<T>(string description, string? default_value,
        List<string>? options, Func<string, T> functor)
    {
        string des_str = description;
        if (default_value != null)
        {
            des_str = String.Format("{0} [{1}]", des_str, default_value);
        }

        if (options != null)
        {
            des_str = String.Format(
                "{0} ({1})", des_str, String.Join(",", options));
        }

        Console.Write(String.Format("{0}: ", des_str));
        string? user_input = Console.ReadLine();

        string value;
        if (String.IsNullOrWhiteSpace(user_input))
        {
            if (default_value == null)
            {
                throw new Exception("Required input not provided!");
            }
            else
            {
                value = default_value;
            }
        }
        else
        {
            value = user_input;
        }

        if (functor == null)
        {
            throw new Exception("Type conversion function not provided!");
        }
        else
        {
            return functor(value);
        }
    }
}
