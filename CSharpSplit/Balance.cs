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

/// <summary>Balance class for a temporary transfer (not linked).</summary>
public class Balance : Transfer
{
    /// <summary>Initialize a Balance object.</summary>
    public Balance(Group group, string purchaser, string recipient,
        double amount, TimeStamp date, Currency currency)
        : base(group, "Pending balance", purchaser, recipient,
            amount, currency, date)
    { }

    /// <summary>Do not link a Balance.</summary>
    protected override void Link()
    {
    }

    /// <summary>Convert Balance to a linked Transfer.</summary>
    /// <returns>A Transfer object.</returns>
    public Transfer ToTransfer()
    {
        string recipient = recipients.Keys.First();
        return group.AddTransfer(
            title, purchaser.name, recipient, amount, currency, date);
    }

    /// <summary>Nothing to unlink in a Balance.</summary>
    protected override void Unlink()
    {
    }
}