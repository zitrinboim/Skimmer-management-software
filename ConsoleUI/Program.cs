using System;

namespace ConsoleUI
{
    class Program
    {
        enum Choices { Add = 1, updating, c, d };
        static void Main(string[] args)
        {
            Choices choices;
            int choice;
            do{
                Console.WriteLine("enter 1 for insert options");
                Console.WriteLine("enter 2 for update options");
                Console.WriteLine("enter 3 for display options");
                Console.WriteLine("enter 4 for List view options");
                Console.WriteLine("enter 0 for EXIT");
                int.TryParse(Console.ReadLine(),out choice);
                choices = choice;
                switch (choices)
                {
                    case Choices.Add:
                        break;
                    case Choices.updating:
                        break;
                    case Choices.c:
                        break;
                    case Choices.d:
                        break;
                    default:
                        break;
                }



            } while (choice != 0) ;

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
