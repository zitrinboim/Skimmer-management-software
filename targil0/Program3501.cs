using System;

namespace targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome3501();
            welcome0124();
        }

        static partial void welcome0124();
        private static void welcome3501()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, Welcome to my first console application", name);
        }
    }
}
