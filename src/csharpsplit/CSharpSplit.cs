namespace CSharpSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            Group test = new("misc/example.json");
            test.Print();
            // test.Save("test.json");
        }
    }
}