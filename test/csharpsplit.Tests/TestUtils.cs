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
namespace csharpsplit.Tests;

using Xunit;
using System;
using System.Collections.Generic;
using CSharpSplit.Utils;

public class TestUtils
{
    private static string date_string = "23.02.2022";
    private static string date_time_string = String.Format(
        "{0} 00:30:00", date_string);

    [Fact]
    public void TestBase()
    {
        // Test: construction
        Base base_obj = new();

        // Test: set the time
        base_obj.SetTime(DateTime.Now);
        base_obj.SetTime(date_time_string);
        base_obj.SetTime(date_string);

        // Test: ToDictionary()
        Dictionary<string, object> tmp = base_obj.ToDictionary();
        Assert.True(tmp.ContainsKey("stamp"));

        // Test: ToString()
        Console.WriteLine(base_obj.ToString());
    }

    [Fact]
    public void TestCurrency()
    {
        foreach (Currency currency in Currency.GetAll<Currency>())
        {
            Console.WriteLine(currency.name);
            Currency.FromName<Currency>(currency.name);
            Currency.FromValue<Currency>(currency.value);
        }
    }

    [Fact]
    public void TestStamp()
    {
        // Test: overload construction
        DateTime now = DateTime.Now;

        Stamp stamp_1 = new();
        Stamp stamp_2 = new(now);
        Stamp stamp_3 = new(date_time_string);

        // Test: set short date string
        Console.WriteLine(stamp_1);
        stamp_1.SetTime(date_string);
        Console.WriteLine(stamp_1);

        // Test: get time object
        DateTime time = stamp_2.GetTime();
        Assert.True(time.Equals(now));

        // Test: ToString()
        Assert.True(date_string.Equals(stamp_1.ToString()));
        Assert.True(date_time_string.Equals(stamp_3.ToString()));

        // Test: raise DateTimeParseException
        try
        {
            String false_date_string = "01.02.22";
            stamp_2.SetTime(false_date_string);
            throw new Exception("Exception not captured!");
        }
        catch (System.FormatException e)
        {
            Console.WriteLine(String.Format("Catched: {0}!", e.GetType().Name));
        }
    }

    [Fact]
    public void TestUtilities()
    {
        // Test: AtLeast1D
        double value = 2.0;
        List<double> tmp = Utilities.AtLeast1D(value);
        tmp = Utilities.AtLeast1D(tmp);
    }
}