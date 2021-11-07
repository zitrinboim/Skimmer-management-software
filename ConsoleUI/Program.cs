//yair busso & shimon zitrinboim.
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
            DalObject dalProgram = new DalObject();
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
                        Add.insertOptions();
                        break;
                    case Choices.UPDATING:
                        Updating.UpdatingOptions();
                        break;
                    case Choices.PISPLAY:
                        Pisplay.PisplayOptions();
                        break;
                    case Choices.LIST_VIEW:
                        ListView.ListViewOptions();
                        break;
                    case Choices.EXIT:
                        Console.WriteLine("Thank you and goodbye");
                        return;
                    default:
                        Console.WriteLine("ERROR! Select again");
                        break;
                }
            } while (choice != 0);
        }
    }
}