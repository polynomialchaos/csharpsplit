namespace CSharpSplit;

using CSharpSplit.Utils;

class Member : Base
{
    public string name { get; private set; }
    private List<Purchase> participations = new();

    public Member(string name)
    {
        this.name = name;
    }

    public void AddParticipation(Purchase purchase)
    {
        participations.Add(purchase);
    }

    public double GetBalance()
    {
        double balance = 0.0;
        foreach (Purchase participation in participations)
        {
            if (participation.IsPurchaser(name))
            {
                balance += participation.getAmount();
            }

            if (participation.IsRecipient(name))
            {
                balance -= participation.getAmountPerMember();
            }
        }

        return balance;
    }

    public void RemoveParticipation(Purchase participation)
    {
        participations.Remove(participation);
    }

    protected override Dictionary<string, Object> Serialize()
    {
        Dictionary<string, Object> tmp = new();
        tmp.Add("name", name);
        return tmp;
    }
}