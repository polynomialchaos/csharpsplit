namespace CSharpSplit;

using CSharpSplit.Utils;

class Balance : Transfer
{
    public Balance(Group group, string purchaser, string recipient, double amount,
            Stamp date, Currency currency) : base(group, "Pending balance", purchaser,
            recipient, amount, currency, date)
    { }

    protected override void Link()
    {
    }

    public void ToTransfer()
    {
        string recipient = recipients.Keys.First();
        group.AddTransfer(title, purchaser.name, recipient, amount, currency, date);
    }

    protected override void Unlink()
    {
    }
}