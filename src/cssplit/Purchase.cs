class Purchase : Base
{
    protected Group group;
    protected Member purchaser;
    protected Dictionary<string, Member> recipients;
    protected Double amount;
    protected Stamp date;
    protected string title;
    protected Currency currency;

    public Purchase(Group group, string purchaser, List<string> recipients, Double amount,
            Stamp date, string title, Currency currency)
    {
        this.group = group;
        SetPurchaser(purchaser);
        SetRecipients(recipients);
        this.amount = amount;
        this.date = date;
        this.title = title;
        this.currency = currency;

        Link();
    }

    public Double getAmount()
    {
        return group.Exchange(amount, currency);
    }

    public Double getAmountPerMember()
    {
        return getAmount() / NumberOfRecipients();
    }

    public Boolean IsPurchaser(string name)
    {
        return purchaser.name.Equals(name);
    }

    public Boolean IsRecipient(string name)
    {
        return recipients.ContainsKey(name);
    }

    protected virtual void Link()
    {
        HashSet<Member> members = new HashSet<Member>(recipients.Values);
        members.Add(purchaser);

        foreach (Member member in members)
        {
            member.AddParticipation(this);
        }
    }

    public int NumberOfRecipients()
    {
        return recipients.Count;
    }

    private void SetPurchaser(string purchaser)
    {
        this.purchaser = group.GetMemberByName(purchaser);
    }

    protected override Dictionary<String, Object> Serialize()
    {
        Dictionary<String, Object> tmp = new Dictionary<String, Object>();
        tmp.Add("purchaser", purchaser.name);
        tmp.Add("recipients", recipients.Keys);
        tmp.Add("amount", amount);
        tmp.Add("currency", currency.name);
        tmp.Add("date", date);
        tmp.Add("title", title);
        return tmp;
    }

    private void SetRecipients(List<string> recipients)
    {
        foreach (string recipient in recipients)
        {
            this.recipients.Add(recipient, group.GetMemberByName(recipient));
        }
    }

    public override string ToString()
    {
        return String.Format("{0} ({1}) {2}: {3}{4} -> {5}", title, date,
            purchaser.name, amount, currency, String.Join(", ", recipients.Keys));
    }

    protected virtual void Unlink()
    {
        HashSet<Member> members = new HashSet<Member>(recipients.Values);
        members.Add(purchaser);

        foreach (Member member in members)
        {
            member.RemoveParticipation(this);
        }
    }
}