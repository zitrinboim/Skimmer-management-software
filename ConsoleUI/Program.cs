using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("enter model of the drone");
            string model = Console.ReadLine();
            Console.WriteLine("entermaximum weight for the drone: 1 to ");
            double longitude = Console.Read();
            Console.WriteLine("enter Latitudes of the station ");
            double lattitude = Console.Read();
            Console.WriteLine("enter number of CargeSlots");
            int ChargeSlots = Console.Read();
        }
    }
}
