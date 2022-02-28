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
namespace csharpsplit.Tests;

using Xunit;
using System.IO;
using System.Collections.Generic;
using CSharpSplit;
using CSharpSplit.Utils;

public class TestGroup
{
    private static string path_1 = "../../../res/csharpsplit.json";
    private static string path_2 = "test_group.json";

    [Fact]
    public void TestGroupBuilder()
    {
        // Test: group construction with value
        Group group = new Group("CSharpSplit",
            "A C# package for money pool split development.", Currency.Euro);
        Assert.True(group.GetTurnover() == 0.0);
        group.SetExchangeRate(Currency.USD, 1.19);
        group.SetTime("23.06.2021 07:53:55");

        // Test: add members
        Member member_1 = group.AddMember("member_1");
        member_1.SetTime("23.06.2021 07:53:55");

        Member member_2 = group.AddMember("member_2");
        member_2.SetTime("23.06.2021 07:53:55");

        // Test: add purchases
        Purchase purchase = group.AddPurchase("purchase_1", "member_1",
                new List<string>{"member_1", "member_2"},
                100.0, Currency.Euro, new Stamp("23.06.2021 07:54:09"));
        purchase.SetTime("23.06.2021 07:54:12");

        purchase = group.AddPurchase("purchase_2", "member_1",
                new List<string>{"member_2"},
                100.0, Currency.Euro, new Stamp("23.06.2021 07:54:21"));
        purchase.SetTime("23.06.2021 07:54:22");

        purchase = group.AddPurchase("purchase_3", "member_1",
                new List<string>{"member_1", "member_2"},
                200.0, Currency.USD, new Stamp("23.06.2021 07:57:19"));
        purchase.SetTime("23.06.2021 07:57:19");

        // Test: add purchases
        Transfer transfer = group.AddTransfer(
                "transfer_1", "member_1", "member_1",
                200.0, Currency.USD, new Stamp("23.06.2021 07:57:19"));
        transfer.SetTime("23.06.2021 07:57:19");

        // Test: ToDictionary()
        Dictionary<string, object> tmp = group.ToDictionary();
        Assert.True(tmp.ContainsKey("stamp"));

        // Test: ToString()
        group.Print();
        System.Console.WriteLine(group.ToString());

        // Test: save
        group.Save(path_2);

        // Test: compare
        StreamReader reader_1 = new(path_1);
        string json_string_1 = reader_1.ReadToEnd();
        reader_1.Close();


        StreamReader reader_2 = new(path_2);
        string json_string_2 = reader_2.ReadToEnd();
        reader_2.Close();

        // TODO: Comparer for JsonElement
        Assert.True(json_string_1.Equals(json_string_2));
    }
}