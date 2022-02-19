
class Member : Base
{
    public string name { get; private set; }
    private List<Purchase> participations = new List<Purchase>();

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

    protected override Dictionary<String, Object> Serialize()
    {
        Dictionary<String, Object> tmp = new Dictionary<String, Object>();
        tmp.Add("name", name);
        return tmp;
    }
}