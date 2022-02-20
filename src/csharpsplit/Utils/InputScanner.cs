class InputScanner
{
    public InputScanner() { }

    public static T Get<T>(string description, Func<string, T> functor) {
        return Get(description, null, null, functor);
    }

    public static T Get<T>(string description, string default_value, Func<string, T> functor) {
        return Get(description, default_value, null, functor);
    }

    public static T Get<T>(string description, string? default_value,
        List<string>? options, Func<string, T> functor)
    {
        string des_str = description;
        if (default_value != null) {
            des_str = String.Format("{0} [{1}]", des_str, default_value);
        }

        if (options != null) {
            des_str = String.Format("{0} ({1})", des_str, String.Join(",", options));
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
