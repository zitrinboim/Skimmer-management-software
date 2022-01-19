//using System;
//using BO;

//namespace ConsoleUI_BL
//{
//   public class Program
//    {
//        public enum Choices { EXIT = 0, ADD, UPDATING, DISPLAY, LIST_VIEW };

//        static void Main(string[] args)
//        {
//            BO.BL BLProgram = new BO.BL();
//            int choice;
//            Choices choices;
//            do
//            {
//                Console.WriteLine("enter 1 for insert options");
//                Console.WriteLine("enter 2 for update options");
//                Console.WriteLine("enter 3 for display options");
//                Console.WriteLine("enter 4 for List view options");
//                Console.WriteLine("enter 0 for EXIT");
//                int.TryParse(Console.ReadLine(), out choice);
//                choices = (Choices)choice;
//                switch (choices)
//                {
//                    case Choices.ADD:
//                        Add a = new(BLProgram);
//                        a.insertOptions();
//                        break;
//                    case Choices.UPDATING:
//                        Updating u = new(BLProgram);
//                        u.UpdatingOptions();
//                        break;
//                    case Choices.DISPLAY:
//                        Display p = new(BLProgram);
//                        p.DisplayOptions();
//                        break;
//                    case Choices.LIST_VIEW:
//                        ListView l = new(BLProgram);
//                        l.ListViewOptions();
//                        break;
//                    case Choices.EXIT:
//                        Console.WriteLine("Thank you and goodbye");
//                        return;
//                    default:
//                        Console.WriteLine("ERROR! Select again");
//                        break;
//                }
//            } while (choice != 0);
//        }
//    }
//}
