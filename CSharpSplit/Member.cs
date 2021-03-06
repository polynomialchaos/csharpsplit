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

/// <summary>Member class which also links purchases and transfers.</summary>
public class Member : Base
{
    public string name { get; private set; }
    private List<Purchase> participations = new();

    /// <summary>Initialize a Member object.</summary>
    public Member(string name)
    {
        this.name = name;
    }

    /// <summary>Adds a purchase to the participation list.</summary>
    public void AddParticipation(Purchase purchase)
    {
        participations.Add(purchase);
    }

    /// <summary>Gets the balance of the member.</summary>
    /// <returns>A Double value.</returns>
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

    /// <summary>Removes a purchase from the participation list.</summary>
    public void RemoveParticipation(Purchase participation)
    {
        participations.Remove(participation);
    }

    /// <summary>Serializes the object.</summary>
    /// <returns>A Dictionary of type string and object.</returns>
    protected override Dictionary<string, object> Serialize()
    {
        Dictionary<string, object> tmp = new();
        tmp.Add("name", name);
        return tmp;
    }
}