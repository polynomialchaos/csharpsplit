class Base
{
    public Stamp time { get; private set; }

    public Base()
    {
        time = new Stamp();
    }

    public override string ToString()
    {
        return String.Format("{0} ({1})", base.ToString(), time.ToString());
    }

    protected virtual Dictionary<String, Object> Serialize() {
        throw new NotImplementedException();
    }

    public Dictionary<String, Object> ToDictionary() {
        Dictionary<String, Object> tmp = Serialize();
        tmp.Add("stamp", time);
        return tmp;
    }

    public static string ToDebugString(Dictionary<String, Object> dictionary) {
        return "{" +
            String.Join(",", dictionary.Select(it => it.Key + "=" + it.Value).ToArray()) +
        "}";
    }
}