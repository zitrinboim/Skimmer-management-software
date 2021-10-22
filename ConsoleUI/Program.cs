using System;

namespace ConsoleUI
{
    class Program
    {
        public enum Choices { EXIT = 0, ADD, UPDATING, PISPLAY, LIST_VIEW };
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine("enter 1 for insert options");
                Console.WriteLine("enter 2 for update options");
                Console.WriteLine("enter 3 for display options");
                Console.WriteLine("enter 4 for List view options");
                Console.WriteLine("enter 0 for EXIT");
                int.TryParse(Console.ReadLine(), out choice);
                Choices choices;
                choices = (Choices)choice;
                switch (choices)
                {
                    case Choices.ADD:
                        insertOptions();
                        break;
                    case Choices.UPDATING:
                        break;
                    case Choices.PISPLAY:
                        break;
                    case Choices.LIST_VIEW:
                        break;
                    case Choices.EXIT:
                        Console.WriteLine("Thank you and goodbye");
                        break;
                    default:
                        Console.WriteLine("ERROR! Select again");
                        break;
                }
            } while (choice != 0);
        }
        public enum enumInsertOptions { EXIT = 0, AFFILIATION, COLLECTION, SUPPLY, SENDING };
        public static void insertOptions()
        {
            Console.WriteLine("Enter 1 for assign a package to a skimmer");
            Console.WriteLine("Enter 2 for package collection by drone");
            Console.WriteLine("enter 3 for Delivery package to customer");
            Console.WriteLine("enter 4 for Sending a skimmer for charging at the base station");
            Console.WriteLine("enter 0 for EXIT");
            enumInsertOptions EnumInsertOptions;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            EnumInsertOptions = (enumInsertOptions)choice;
            switch (EnumInsertOptions)
            {
                case enumInsertOptions.AFFILIATION:
                    break;
                case enumInsertOptions.COLLECTION:
                    break;
                case enumInsertOptions.SUPPLY:
                    break;
                case enumInsertOptions.SENDING:
                    break;
                case enumInsertOptions.EXIT:
                    break;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }


        }
        //Console.WriteLine("enter model of the drone");
        //string model = Console.ReadLine();
        //Console.WriteLine("entermaximum weight for the drone: 1 to ");
        //double longitude = Console.Read();
        //Console.WriteLine("enter Latitudes of the station ");
        //double lattitude = Console.Read();
        //Console.WriteLine("enter number of CargeSlots");
        //int ChargeSlots = Console.Read();
    }
}
