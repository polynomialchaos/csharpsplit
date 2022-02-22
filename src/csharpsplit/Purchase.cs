// MIT License

// Copyright (c) 2022 Florian Eigentler

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace CSharpSplit;

using CSharpSplit.Utils;

public class Purchase : Base
{
    protected Group group;
    protected Member purchaser = null!;
    protected Dictionary<string, Member> recipients = new();
    protected double amount;
    protected Stamp date;
    protected string title;
    protected Currency currency;

    public Purchase(Group group, string title, string purchaser, List<string> recipients,
        double amount, Currency currency, Stamp date)
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

    public double getAmount()
    {
        return group.Exchange(amount, currency);
    }

    public double getAmountPerMember()
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
        HashSet<Member> members = new(recipients.Values);
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

    protected override Dictionary<string, object> Serialize()
    {
        Dictionary<string, object> tmp = new();
        tmp.Add("purchaser", purchaser.name);
        tmp.Add("recipients", recipients.Keys);
        tmp.Add("amount", amount);
        tmp.Add("currency", currency.name);
        tmp.Add("date", date.ToString());
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
        return String.Format("{0} ({1}) {2}: {3:F2}{4} -> {5}", title, date,
            purchaser.name, amount, currency, String.Join(", ", recipients.Keys));
    }

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