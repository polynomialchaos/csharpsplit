class Base
{
    public Stamp time { get; private set; }

    public Base()
    {
        time = new();
    }

    public override string ToString()
    {
        return String.Format("{0} ({1})", base.ToString(), time.ToString());
    }

    protected virtual Dictionary<string, Object> Serialize() {
        throw new NotImplementedException();
    }

    public Dictionary<string, Object> ToDictionary() {
        Dictionary<string, Object> tmp = Serialize();
        tmp.Add("stamp", time.ToString());
        return tmp;
    }

    public static string ToDebugString(Dictionary<string, Object> dictionary) {
        return "{" +
            String.Join(",", dictionary.Select(it => it.Key + "=" + it.Value).ToArray()) +
        "}";
    }
}