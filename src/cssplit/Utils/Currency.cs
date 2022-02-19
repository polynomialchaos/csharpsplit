public class Currency : Enumeration<string, string>
{
    public static readonly Currency Euro = new Currency("Euro", "€");
    public static readonly Currency USD = new Currency("USD", "$");
    private Currency(string name, string symbol) : base(name, symbol) { }
}