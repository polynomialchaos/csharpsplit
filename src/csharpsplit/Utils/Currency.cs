namespace CSharpSplit.Utils;

public class Currency : Enumeration<string, string>
{
    public static readonly Currency Euro = new("Euro", "€");
    public static readonly Currency USD = new("USD", "$");
    private Currency(string name, string symbol) : base(name, symbol) { }
}