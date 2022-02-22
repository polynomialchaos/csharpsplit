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

public class Member : Base
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

    protected override Dictionary<string, object> Serialize()
    {
        Dictionary<string, object> tmp = new();
        tmp.Add("name", name);
        return tmp;
    }
}