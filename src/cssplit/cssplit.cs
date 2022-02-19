namespace cssplit
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");

            // // Console.WriteLine(Currency.Euro);
            // // Console.WriteLine(false.CompareTo(false));

            // Group base_obj = new Group();
            // // base_obj.time.SetTime("24.01.2002");

            // // Currency.Euro;
            // // Stamp stamp = new Stamp("24.02.2020 00:00:00");
            // // // Stamp stamp = new Stamp();
            // // stamp.time = DateTime.Now;
            // Console.WriteLine(Group.ToDebugString(base_obj.ToDictionary()));

            Member member = new Member("Florian");
            Console.WriteLine(member.ToString());
        }
    }
}