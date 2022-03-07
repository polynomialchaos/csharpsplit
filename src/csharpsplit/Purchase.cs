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

using CSharpSplit.Utils;

/// <summary>Purchase class.</summary>
public class Purchase : Base
{
    protected Group group;
    protected Member purchaser = null!;
    protected OrderdDictionary<string, Member> recipients = new();
    protected double amount;
    protected TimeStamp date;
    protected string title;
    protected Currency currency;

    /// <summary>Initialize a Purchase object.</summary>
    public Purchase(Group group, string title,
        string purchaser, ICollection<string> recipients,
        double amount, Currency currency, TimeStamp date)
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

    /// <summary>Gets the amount in group currency.</summary>
    /// <returns>A double value.</returns>
    public double getAmount()
    {
        return group.Exchange(amount, currency);
    }

    /// <summary>Gets the amount per member in group currency.</summary>
    /// <returns>A double value.</returns>
    public double getAmountPerMember()
    {
        return getAmount() / NumberOfRecipients();
    }

    /// <summary>Checks the name to be the purchaser.</summary>
    /// <returns>A boolean flag.</returns>
    public bool IsPurchaser(string name)
    {
        return purchaser.name.Equals(name);
    }

    /// <summary>Checks the name to be a recipients.</summary>
    /// <returns>A boolean flag.</returns>
    public bool IsRecipient(string name)
    {
        return recipients.ContainsKey(name);
    }

    /// <summary>Link this Purchase in all members.</summary>
    protected virtual void Link()
    {
        HashSet<Member> members = new(recipients.Values);
        members.Add(purchaser);

        foreach (Member member in members)
        {
            member.AddParticipation(this);
        }
    }

    /// <summary>Gets the number of recipients.</summary>
    public int NumberOfRecipients()
    {
        return recipients.Count;
    }

    /// <summary>Set purchaser by name.</summary>
    private void SetPurchaser(string purchaser)
    {
        this.purchaser = group.GetMemberByName(purchaser);
    }

    /// <summary>Serializes the object.</summary>
    /// <returns>A Dictionary of type string and object.</returns>
    protected override Dictionary<string, object> Serialize()
    {
        Dictionary<string, object> tmp = new();
        tmp.Add("purchaser", purchaser.name);
        tmp.Add("recipients", recipients.Keys);
        tmp.Add("amount", amount);
        tmp.Add("currency", currency.key);
        tmp.Add("date", date.ToString());
        tmp.Add("title", title);
        return tmp;
    }

    /// <summary>Set recipients by ICollection of names.</summary>
    private void SetRecipients(ICollection<string> recipients)
    {
        foreach (string recipient in recipients)
        {
            this.recipients.Add(recipient, group.GetMemberByName(recipient));
        }
    }

    /// <summary>Converts to an equivalent string.</summary>
    /// <returns>A string.</returns>
    public override string ToString()
    {
        return String.Format("{0} ({1}) {2}: {3:F2}{4} -> {5}",
            title, date, purchaser.name, amount, currency,
            String.Join(", ", recipients.Keys));
    }

    /// <summary>Unlink this Purchase from all members.</summary>
    protected virtual void Unlink()
    {
        HashSet<Member> members = new(recipients.Values);
        members.Add(purchaser);

        foreach (Member member in members)
        {
            member.RemoveParticipation(this);
        }
    }
}