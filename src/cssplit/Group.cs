class Group : Base
{
    private string name;
    private string description;
    private Currency currency;
    private Dictionary<Currency, Double> exchange_rates = new Dictionary<Currency, Double>();
    private Dictionary<string, Member> members = new Dictionary<string, Member>();
    private List<Purchase> purchases = new List<Purchase>();
    private List<Transfer> transfers = new List<Transfer>();

    public Group(string name, string description, Currency currency)
    {
        this.name = name;
        this.description = description;
        this.currency = currency;
    }

    public Group(string name)
    {
        // this.name = name;
        // this.description = description;
        // this.currency = currency;
    }

    public Member AddMember(string name)
    {
        if (name.Trim().Length == 0)
        {
            throw new Exception("Empty member name provided!");
        }

        if (members.ContainsKey(name))
        {
            throw new Exception("Duplicate member name provided!");
        }

        Member member = new Member(name);
        members.Add(name, member);

        return member;
    }

    public Purchase AddPurchase(string purchaser, List<string> recipients, Double amount,
            Stamp date, string title, Currency currency)
    {
        Purchase purchase = new Purchase(this, purchaser, recipients, amount, date, title, currency);
        purchases.Add(purchase);

        return purchase;
    }

    public Transfer AddTransfer(string purchaser, string recipient, Double amount,
            Stamp date, string title, Currency currency)
    {
        Transfer transfer = new Transfer(this, purchaser, recipient, amount, date, title, currency);
        transfers.Add(transfer);

        return transfer;
    }

    public double Exchange(double amount, Currency currency)
    {
        if (currency.Equals(this.currency))
        {
            return amount;
        }
        else
        {
            return amount / exchange_rates[currency];
        }
    }

    public Member GetMemberByName(string name)
    {
        return members[name];
    }

    public List<string> GetMemberNames()
    {
        return members.Keys.ToList();
    }

    public int GetNumberOfMembers()
    {
        return members.Count;
    }

    public static IEnumerable<T> LazyReverse<T>(IList<T> items)
    {
        for (var i = items.Count - 1; i >= 0; i--)
            yield return items[i];
    }

    public List<Balance> GetPendingBalances()
    {
        List<Balance> balances = new List<Balance>();

        // sorted members
        List<Member> members = this.members.Values.ToList();
        members.Sort(delegate (Member x, Member y)
        {
            return x.GetBalance().CompareTo(y.GetBalance());
        });

        Dictionary<Member, Double> bal_add = new Dictionary<Member, Double>();
        foreach (Member member in members)
        {
            bal_add.Add(member, 0.0);
        }

        foreach (Member sender in members)
        {
            foreach (Member receiver in LazyReverse(members))
            {
                if (sender == receiver)
                    continue;

                double sender_balance = sender.GetBalance() + bal_add[sender];
                double receiver_balance = receiver.GetBalance() + bal_add[receiver];

                if (receiver_balance > 0.0)
                {
                    Double bal = Math.Min(Math.Abs(sender_balance), receiver_balance);
                    bal_add.Add(sender, bal_add[sender] + bal);
                    bal_add.Add(receiver, bal_add[receiver] - bal);

                    Balance balance = new Balance(this, sender.name, receiver.name,
                        bal, new Stamp(), currency);
                    balances.Add(balance);
                }
            }
        }

        return balances;
    }

    public double GetTurnover()
    {
        double turnover = 0.0;
        foreach (Purchase purchase in purchases)
        {
            turnover += purchase.getAmount();
        }

        return turnover;
    }

    public void Print()
    {
        int length = 80;
        string mainrule = new String('=', length);
        string rule = new String('-', length);

        Console.WriteLine(mainrule);
        Console.WriteLine(String.Format("Summary for group: %s", name));
        if (description.Length > 0)
        {
            Console.WriteLine(description);
        }

        Console.WriteLine(mainrule);
        Console.WriteLine(String.Format(" * Turnover: %.2f%s", GetTurnover(), currency));

        if (exchange_rates.Count > 0)
        {
            Console.WriteLine(rule);
            Console.WriteLine("Exchange rates:");
            foreach (KeyValuePair<Currency, double> item in exchange_rates)
            {
                Console.WriteLine(String.Format(" * 1{0} -> {1}{2}",
                    currency, item.Value, item.Key));
            }
        }

        Console.WriteLine(rule);
        Console.WriteLine("Members:");
        foreach (Member member in members.Values)
        {
            Console.WriteLine(String.Format(" * %s", member.name));
        }

        Console.WriteLine(rule);
        Console.WriteLine("Purchases:");
        foreach (Purchase purchase in purchases)
        {
            Console.WriteLine(String.Format(" * %s", purchase));
        }

        Console.WriteLine(rule);
        Console.WriteLine("Transfers:");
        foreach (Transfer transfer in transfers)
        {
            Console.WriteLine(String.Format(" * %s", transfer));
        }

        Console.WriteLine(rule);
        Console.WriteLine("Pending balances:");
        foreach (Balance balance in GetPendingBalances())
        {
            Console.WriteLine(String.Format(" * %s", balance));
        }

        Console.WriteLine(mainrule);
    }

    public void Save(string path)
    {
        // try {
        //     Gson gson = new GsonBuilder().setPrettyPrinting().create();
        //     FileWriter writer = new FileWriter(path);
        //     gson.toJson(this.toDict(), writer);
        //     writer.flush();
        //     writer.close();
        // } catch (Exception e) {
        //     e.printStackTrace();
        // }
    }

    protected override Dictionary<string, Object> Serialize()
    {
        Dictionary<string, Double> exchange_rates = new Dictionary<string, Double>();
        foreach (KeyValuePair<Currency, double> item in this.exchange_rates)
        {
            exchange_rates.Add(item.Key.name, item.Value);
        }

        Dictionary<string, Object> tmp = new Dictionary<string, Object>();
        tmp.Add("name", name);
        tmp.Add("description", description);
        tmp.Add("currency", currency.name);
        tmp.Add("members", members.Values.ToList().ConvertAll(member => member.ToDictionary()));
        tmp.Add("purchases", purchases.ConvertAll(purchase => purchase.ToDictionary()));
        tmp.Add("transfers", transfers.ConvertAll(transfer => transfer.ToDictionary()));
        tmp.Add("exchange_rates", exchange_rates);
        return tmp;
    }
}