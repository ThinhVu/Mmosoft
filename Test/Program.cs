using System;
using Mmosoft.Syntax.PrivateStore;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Person();
            // store value 5 (int) in private store with prop "age"
            p.Set("age", 5);
            // get value from prop "age"
            p.Get<int>("age");

            Console.Read();
        }
    }

    public class Person {}
}
