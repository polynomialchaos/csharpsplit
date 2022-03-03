// MIT License

// Copyright (c) 2022 Florian Eigentler

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace CSharpSplit;

using System.Text.Json;
using CSharpSplit.Utils;

public class Group : Base
{
    public string name { get; private set; } = String.Empty;
    private string description = String.Empty;
    public Currency currency { get; set; } = Currency.Euro;
    private OrderdDictionary<Currency, double> exchange_rates = new();
    private OrderdDictionary<string, Member> members = new();
    private List<Purchase> purchases = new();
    private List<Transfer> transfers = new();

    public Group(string name, string description, Currency currency)
    {
        this.name = name;
        this.description = description;
        this.currency = currency;
    }

    public Group(string path)
    {
        StreamReader reader = new(path);
        string json_string = reader.ReadToEnd();
        reader.Close();

        JsonElement json_object =
            JsonSerializer.Deserialize<JsonElement>(json_string);
        Dictionary<string, JsonElement> json_root =
            FromJSON<Dictionary<string, JsonElement>>(json_object);

        // group
        name = FromJSON<string>(json_root["name"]);
        description = FromJSON<string>(json_root["description"]);
        string currency_string = FromJSON<string>(json_root["currency"]);
        currency = Currency.FromName<Currency>(currency_string);
        time.SetTime(FromJSON<string>(json_root["stamp"]));

        // exchange rates
        Dictionary<string, double> json_exchange_rates =
            FromJSON<Dictionary<string, double>>(
                json_root["exchange_rates"]);
        foreach (Currency item in Currency.GetAll<Currency>())
        {
            if (json_exchange_rates.ContainsKey(item.name))
            {
                this.exchange_rates.Add(item, json_exchange_rates[item.name]);
            }
        }

        // members
        List<JsonElement> json_members =
            FromJSON<List<JsonElement>>(json_root["members"]);
        foreach (JsonElement json_member_it in json_members)
        {
            Dictionary<string, string> json_member =
                FromJSON<Dictionary<string, string>>(json_member_it);

            Member tmp = AddMember(json_member["name"]);
            tmp.SetTime(json_member["stamp"]);
        }

        // purchases
        List<JsonElement> json_purchases =
            FromJSON<List<JsonElement>>(json_root["purchases"]);
        foreach (JsonElement json_purchase_it in json_purchases)
        {
            Dictionary<string, JsonElement> json_purchase =
                FromJSON<Dictionary<string, JsonElement>>(
                    json_purchase_it);

            string purchaser = FromJSON<string>(json_purchase["purchaser"]);
            List<string> recipients =
                FromJSON<List<string>>(json_purchase["recipients"]);
            double amount = FromJSON<double>(json_purchase["amount"]);
            string date_string = FromJSON<string>(json_purchase["date"]);
            Stamp date = new(date_string);
            string title = FromJSON<string>(json_purchase["title"]);
            string tmp = FromJSON<string>(json_purchase["currency"]);
            Currency currency = Currency.FromName<Currency>(tmp);

            Purchase purchase = AddPurchase(
                title, purchaser, recipients, amount, currency, date);
            purchase.SetTime(FromJSON<string>(json_purchase["stamp"]));
        }

        // transfers
        List<JsonElement> json_transfers =
            FromJSON<List<JsonElement>>(json_root["transfers"]);
        foreach (JsonElement json_transfer_it in json_transfers)
        {
            Dictionary<string, JsonElement> json_transfer =
                FromJSON<Dictionary<string, JsonElement>>(
                    json_transfer_it);

            string purchaser = FromJSON<string>(json_transfer["purchaser"]);
            List<string> recipients =
                FromJSON<List<string>>(json_transfer["recipients"]);
            double amount = FromJSON<double>(json_transfer["amount"]);
            string date_string = FromJSON<string>(json_transfer["date"]);
            Stamp date = new(date_string);
            string title = FromJSON<string>(json_transfer["title"]);
            string tmp = FromJSON<string>(json_transfer["currency"]);
            Currency currency = Currency.FromName<Currency>(tmp);

            Transfer transfer = AddTransfer(title, purchaser, recipients[0],
                amount, currency, date);
            transfer.SetTime(FromJSON<string>(json_transfer["stamp"]));
        }
    }

    public Member AddMember(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Empty member name provided!");
        }

        if (members.ContainsKey(name))
        {
            throw new Exception("Duplicate member name provided!");
        }

        Member member = new(name);
        members.Add(name, member);

        return member;
    }

    public Purchase AddPurchase(string title, string purchaser,
        List<string> recipients, double amount, Currency currency, Stamp date)
    {
        Purchase purchase = new(
            this, title, purchaser, recipients, amount, currency, date);
        purchases.Add(purchase);

        return purchase;
    }

    public Transfer AddTransfer(string title, string purchaser,
        string recipient, double amount, Currency currency, Stamp date)
    {
        Transfer transfer = new(
            this, title, purchaser, recipient, amount, currency, date);
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

    private static T FromJSON<T>(JsonElement element)
    {
        var tmp = element.Deserialize<T>();
        if (tmp == null)
        {
            throw new Exception("JSON deserialization returns null!");
        }

        return tmp;
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

    public List<Balance> GetPendingBalances()
    {
        List<Balance> balances = new();

        // sorted members
        List<Member> members = this.members.Values.ToList();
        members.Sort(delegate (Member x, Member y)
        {
            return x.GetBalance().CompareTo(y.GetBalance());
        });

        OrderdDictionary<Member, double> bal_add = new();
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
                double receiver_balance =
                    receiver.GetBalance() + bal_add[receiver];

                if (receiver_balance > 0.0)
                {
                    double bal = Math.Min(
                        Math.Abs(sender_balance), receiver_balance);
                    bal_add[sender] += bal;
                    bal_add[receiver] -= bal;

                    Balance balance = new(this, sender.name, receiver.name,
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

    private static IEnumerable<T> LazyReverse<T>(IList<T> items)
    {
        for (int i = items.Count - 1; i >= 0; i--)
            yield return items[i];
    }

    public void Print()
    {
        int length = 80;
        string mainrule = new('=', length);
        string rule = new('-', length);

        Console.WriteLine(mainrule);
        Console.WriteLine(String.Format("Summary for group: {0}", name));
        if (!String.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine(description);
        }

        Console.WriteLine(mainrule);
        Console.WriteLine(String.Format(
            " * Turnover: {0:F2}{1}", GetTurnover(), currency.ToString()));

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
            Console.WriteLine(String.Format(" * {0}", member.name));
        }

        Console.WriteLine(rule);
        Console.WriteLine("Purchases:");
        foreach (Purchase purchase in purchases)
        {
            Console.WriteLine(String.Format(" * {0}", purchase));
        }

        Console.WriteLine(rule);
        Console.WriteLine("Transfers:");
        foreach (Transfer transfer in transfers)
        {
            Console.WriteLine(String.Format(" * {0}", transfer));
        }

        Console.WriteLine(rule);
        Console.WriteLine("Pending balances:");
        foreach (Balance balance in GetPendingBalances())
        {
            Console.WriteLine(String.Format(" * {0}", balance));
        }

        Console.WriteLine(mainrule);
    }

    public void SetExchangeRate(Currency currency, double rate)
    {
        exchange_rates[currency] = rate;
    }

    public void Save(string path)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string jsonString =
            JsonSerializer.Serialize(this.ToDictionary(), options);
        File.WriteAllText(path, jsonString);
    }

    protected override Dictionary<string, object> Serialize()
    {
        Dictionary<string, double> exchange_rates = new();
        foreach (KeyValuePair<Currency, double> item in this.exchange_rates)
        {
            exchange_rates.Add(item.Key.name, item.Value);
        }

        Dictionary<string, object> tmp = new();
        tmp.Add("name", name);
        tmp.Add("description", description);
        tmp.Add("currency", currency.name);
        tmp.Add("members", members.Values.ToList().ConvertAll(
            member => member.ToDictionary()));
        tmp.Add("purchases", purchases.ConvertAll(
            purchase => purchase.ToDictionary()));
        tmp.Add("transfers", transfers.ConvertAll(
            transfer => transfer.ToDictionary()));
        tmp.Add("exchange_rates", exchange_rates);
        return tmp;
    }
}