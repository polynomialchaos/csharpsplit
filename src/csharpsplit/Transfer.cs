namespace CSharpSplit;

using CSharpSplit.Utils;

class Transfer : Purchase
{
    public Transfer(Group group, string title, string purchaser, string recipient,
        double amount, Currency currency, Stamp date) : base(group, title,
            purchaser, AsList(recipient), amount, currency, date)
    { }

    public static List<T> AsList<T>(T item)
    {
        List<T> list = new();
        list.Add(item);
        return list;
    }
}