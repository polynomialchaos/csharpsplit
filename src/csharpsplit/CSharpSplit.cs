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

using System.CommandLine;
using CSharpSplit.Utils;

/// <summary>Program class implementing main entry point.</summary>
public class Program
{
    static void Main(string[] args)
    {
        Option<bool> exchange_option = new(
                "--exchange",
                "Add exchange rate(s) to the group.");
        exchange_option.AddAlias("-e");

        Option<bool> member_option = new(
                "--member",
                "Add member(s) to the group.");
        member_option.AddAlias("-m");

        Option<bool> purchase_option = new(
                "--purchase",
                "Add purchase(s) to the group.");
        purchase_option.AddAlias("-p");

        Option<bool> transfer_option = new(
                "--transfer",
                "Add transfer(s) to the group.");
        transfer_option.AddAlias("-t");

        Argument<string> file_path_argument = new();
        file_path_argument.Arity = ArgumentArity.ZeroOrOne;

        RootCommand rootCommand = new("CSharpSplit")
        {
            exchange_option,
            member_option,
            purchase_option,
            transfer_option,
            file_path_argument
        };

        rootCommand.SetHandler((bool add_exchange, bool add_member,
            bool add_purchase, bool add_transfer, string file_path) =>
        {
            // group
            Group group;
            if (file_path != null)
            {
                group = new(file_path);
            }
            else
            {
                string inp_title = InputScanner.Get(
                    "Group title", "Untitled", a => a);
                string inp_description = InputScanner.Get(
                    "Group description", "", a => a);
                Currency inp_currency = InputScanner.Get(
                    "Group currency", Currency.Euro.key,
                    Currency.GetAll<Currency>().ToList().ConvertAll(
                        a => a.key),
                    a => Currency.FromName<Currency>(a));

                group = new(inp_title, inp_description, inp_currency);
            }

            // add exchange(s)
            if (add_exchange)
            {
                List<Currency> currencies =
                    Currency.GetAll<Currency>().ToList().FindAll(
                        it => it != group.currency
                    );

                while (currencies.Count > 0)
                {
                    Currency inp_currency = InputScanner.Get(
                        "Exchange rate currency", currencies[0].key,
                        currencies.ConvertAll(a => a.key),
                        a => Currency.FromName<Currency>(a));
                    double inp_rate = InputScanner.Get(
                        String.Format("{0} exchange rate", inp_currency.key),
                        a => Double.Parse(a));

                    group.SetExchangeRate(inp_currency, inp_rate);

                    if (!InputScanner.Get("Add another exchange rate", "n",
                        new List<string> { "y", "n" },
                        a => a.ToLowerInvariant() == "y"))
                    {
                        break;
                    }
                }
            }

            // add member(s)
            if (add_member || group.GetNumberOfMembers() == 0)
            {
                while (true)
                {
                    string inp_name = InputScanner.Get(
                        "Member name (Enter to continue)",
                        "", a => a);

                    if (String.IsNullOrWhiteSpace(inp_name))
                    {
                        break;
                    }
                    else
                    {
                        group.AddMember(inp_name);
                    }
                }
            }

            // add purchase(s)
            if (add_purchase)
            {
                if (group.GetNumberOfMembers() == 0)
                {
                    throw new Exception("No members have been defined!");
                }

                List<string> members = group.GetMemberNames();
                while (true)
                {
                    string inp_title = InputScanner.Get(
                        "Purchase title", "Untitled", a => a);
                    string inp_purchaser = InputScanner.Get(
                        "Purchaser", members[0], members, a => a);
                    List<string> inp_recipients = InputScanner.Get(
                        "Purchase recipients (seperated by ;)",
                        String.Join(";", members), members,
                        a => a.Split(";").ToList());
                    double inp_amount = InputScanner.Get(
                        "Purchase amount", a => Double.Parse(a));
                    Currency inp_currency = InputScanner.Get(
                        "Purchase currency", group.currency.key,
                        Currency.GetAll<Currency>().ToList().ConvertAll(
                            a => a.key),
                        a => Currency.FromName<Currency>(a));
                    TimeStamp inp_date = InputScanner.Get(
                        "Purchase date",
                        new TimeStamp().ToString(),
                        a => new TimeStamp(a));

                    group.AddPurchase(inp_title, inp_purchaser, inp_recipients,
                            inp_amount, inp_currency, inp_date);

                    if (!InputScanner.Get("Add another purchase", "n",
                        new List<string> { "y", "n" },
                        a => a.ToLowerInvariant() == "y"))
                    {
                        break;
                    }
                }
            }

            // add transfer(s)
            if (add_transfer)
            {
                if (group.GetNumberOfMembers() == 0)
                {
                    throw new Exception("No members have been defined!");
                }

                List<string> members = group.GetMemberNames();
                while (true)
                {
                    string inp_title = InputScanner.Get(
                        "Transfer title", "Untitled", a => a);
                    string inp_purchaser = InputScanner.Get(
                        "Purchaser", members[0], members, a => a);
                    string inp_recipient = InputScanner.Get(
                        "Transfer recipient", members[0], members, a => a);
                    double inp_amount = InputScanner.Get(
                        "Transfer amount", a => Double.Parse(a));
                    Currency inp_currency = InputScanner.Get(
                        "Transfer currency", group.currency.key,
                        Currency.GetAll<Currency>().ToList().ConvertAll(
                            a => a.key),
                        a => Currency.FromName<Currency>(a));
                    TimeStamp inp_date = InputScanner.Get(
                        "Transfer date",
                        new TimeStamp().ToString(),
                        a => new TimeStamp(a));

                    group.AddTransfer(inp_title, inp_purchaser, inp_recipient,
                            inp_amount, inp_currency, inp_date);

                    if (!InputScanner.Get("Add another transfer", "n",
                        new List<string> { "y", "n" },
                        a => a.ToLowerInvariant() == "y"))
                    {
                        break;
                    }
                }
            }

            // print the group stats
            group.Print();

            // store the group in the existing file or create a new one
            if (file_path == null)
            {
                file_path = group.name.ToLowerInvariant().Replace(" ", "_");
                file_path = InputScanner.Get(
                    "Provide file name", file_path + ".json", a => a);
            }
            group.Save(file_path);
        }, exchange_option, member_option, purchase_option,
            transfer_option, file_path_argument);

        rootCommand.Invoke(args);
    }
}