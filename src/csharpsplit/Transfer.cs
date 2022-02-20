class Transfer : Purchase
{
    public Transfer(Group group, string purchaser, string recipient, double amount,
            Stamp date, string title, Currency currency) : base(group, purchaser,
            AsList(recipient), amount, date, title, currency)
    { }

    public static List<T> AsList<T>(T item)
    {
        List<T> list = new();
        list.Add(item);
        return list;
    }
}