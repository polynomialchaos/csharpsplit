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

using System.Globalization;

/// <summary>TimeStamp class for storing time information.</summary>
public class TimeStamp
{
    private static IFormatProvider culture_info = new CultureInfo("de-DE");
    private static string fmt_date_time = "dd.MM.yyyy HH:mm:ss";
    private static string fmt_date = "dd.MM.yyyy";
    public DateTime time { get; private set; }

    /// <summary>Initialize a TimeStamp object.</summary>
    public TimeStamp()
    {
        SetTime(DateTime.Now);
    }

    /// <summary>Initialize a TimeStamp object from DateTime object.</summary>
    public TimeStamp(DateTime time)
    {
        SetTime(time);
    }

    /// <summary>Initialize a TimeStamp object from time string.</summary>
    public TimeStamp(string time_string)
    {
        SetTime(time_string);
    }

    /// <summary>Sets the time from a DateTime object.</summary>
    public void SetTime(DateTime time)
    {
        this.time = time;
    }

    /// <summary>Sets the time from a string.</summary>
    public void SetTime(string time_string)
    {
        try
        {
            time = DateTime.ParseExact(
                time_string, fmt_date_time, culture_info);
        }
        catch (System.Exception)
        {
            time = DateTime.ParseExact(
                time_string, fmt_date, culture_info);
        }
    }

    /// <summary>Checks if the time is at start of the day (00:00).</summary>
    /// <returns>A boolean flag.</returns>
    private bool IsStartOfDay()
    {
        return (time.Hour == 0) && (time.Minute == 0) &&
            (time.Second == 0) && (time.Millisecond == 0);

    }

    /// <summary>Converts to an equivalent string.</summary>
    /// <returns>A string.</returns>
    public override string ToString()
    {
        return time.ToString(IsStartOfDay() ? fmt_date : fmt_date_time);
    }
}