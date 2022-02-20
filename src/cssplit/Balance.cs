class Balance : Transfer
{
    public Balance(Group group, string purchaser, string recipient, double amount,
            Stamp date, Currency currency) : base(group, purchaser,
            recipient, amount, date, "Pending balance", currency)
    { }

    protected override void Link()
    {
    }

    public void ToTransfer()
    {
        string recipient = recipients.Keys.First();
        group.AddTransfer(purchaser.name, recipient, amount, date, title, currency);
    }

    protected override void Unlink()
    {
    }
}