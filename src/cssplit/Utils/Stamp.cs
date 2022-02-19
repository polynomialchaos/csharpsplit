using System.Globalization;

class Stamp
{
    private static IFormatProvider culture_info = new CultureInfo("de-DE");
    private static string fmt_date_time = "dd.MM.yyyy HH:mm:ss";
    private static string fmt_date = "dd.MM.yyyy";
    private DateTime time;

    public Stamp()
    {
        SetTime(DateTime.Now);
    }

    public Stamp(string time_string)
    {
        SetTime(time_string);
    }

    public DateTime GetTime()
    {
        return time;
    }

    public void SetTime(DateTime time)
    {
        this.time = time;
    }

    public void SetTime(string time_string)
    {
        try
        {
            time = DateTime.ParseExact(time_string, fmt_date_time, culture_info);
        }
        catch (System.Exception)
        {
            time = DateTime.ParseExact(time_string, fmt_date, culture_info);
        }
    }

    private Boolean IsStartOfDay()
    {
        return (time.Hour == 0) && (time.Minute == 0) &&
            (time.Second == 0) && (time.Millisecond == 0);

    }

    public override string ToString()
    {
        return time.ToString(IsStartOfDay() ? fmt_date : fmt_date_time);
    }
}