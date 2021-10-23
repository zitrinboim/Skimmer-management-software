using System;
using IDAL.DO;
using Dal;

namespace ConsoleUI
{
    class Program
    {
        public enum Choices { EXIT = 0, ADD, UPDATING, PISPLAY, LIST_VIEW };
        static void Main(string[] args)
        {
            int choice;
            Choices choices;
            do
            {
                Console.WriteLine("enter 1 for insert options");
                Console.WriteLine("enter 2 for update options");
                Console.WriteLine("enter 3 for display options");
                Console.WriteLine("enter 4 for List view options");
                Console.WriteLine("enter 0 for EXIT");
                int.TryParse(Console.ReadLine(), out choice);
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
        public enum enumInsertOptions { EXIT = 0, ADD_STATION, ADD_DRONE, ADD_CUSTOMER, ADD_PARCEL };
        public static void insertOptions()
        {

            enumInsertOptions EnumInsertOptions;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            EnumInsertOptions = (enumInsertOptions)choice;
            switch (EnumInsertOptions)
            {
                case enumInsertOptions.ADD_STATION:
                    ADD_STATION();
                    break;
                case enumInsertOptions.ADD_DRONE:
                    break;
                case enumInsertOptions.ADD_CUSTOMER:
                    break;
                case enumInsertOptions.ADD_PARCEL:
                    break;
                case enumInsertOptions.EXIT:
                    break;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }
            //Console.WriteLine("enter model of the drone");
            //string model = Console.ReadLine();
            //Console.WriteLine("entermaximum weight for the drone: 1 to ");
            //double longitude = Console.Read();
            //Console.WriteLine("enter Latitudes of the station ");
            //double lattitude = Console.Read();
            //Console.WriteLine("enter number of CargeSlots");
            //int ChargeSlots = Console.Read();


            //Console.WriteLine("Enter 1 for assign a package to a skimmer");
            //Console.WriteLine("Enter 2 for package collection by drone");
            //Console.WriteLine("enter 3 for Delivery package to customer");
            //Console.WriteLine("enter 4 for Sending a skimmer for charging at the base station");
            //Console.WriteLine("enter 0 for EXIT");
        }
        public static void ADD_STATION()
        {
            Console.WriteLine("enter id ");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("enter name of station");
            string name = Console.ReadLine();
            Console.WriteLine("enter longitude of the station ");
            double longitude;
            double.TryParse(Console.ReadLine(), out longitude);
            Console.WriteLine("enter Latitudes of the station ");
            double lattitude;
            double.TryParse(Console.ReadLine(), out lattitude);
            Console.WriteLine("enter number of CargeSlots");
            int ChargeSlots;
            int.TryParse(Console.ReadLine(), out ChargeSlots);
            Station station = new()
            {
                Id = id,
                name = name,
                longitude = longitude,
                lattitude = lattitude,
                ChargeSlots = ChargeSlots
            };
            bool test = Dal.DalObject.addStation(station);
            if (test == false)
                Console.WriteLine("ERROR");
        }
    }
}
