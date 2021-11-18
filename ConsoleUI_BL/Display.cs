using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
   public class Display
    {
        IBL.BO.BL BLProgram;
        public Display(IBL.BO.BL _bLProgram)
        {
            BLProgram = _bLProgram;
        }

        public enum enumDisplayOptions { EXIT = 0, STATION_DISPLAY, DRONE_DISPLAY, CUSTOMER_DISPLAY, PARCEL_DISPLAY, };
        public void DisplayOptions()
        {
            Console.WriteLine("enter 1 to  get the information on station");
            Console.WriteLine("enter 2 to  get the information on drone");
            Console.WriteLine("enter 3 to  get the information on customer");
            Console.WriteLine("enter 4 to  get the information on parcel");
            Console.WriteLine("enter 0 to EXIT");

            enumDisplayOptions enumUpdating;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            enumUpdating = (enumDisplayOptions)choice;
            switch (enumUpdating)
            {
                case enumDisplayOptions.STATION_DISPLAY:
                    STATION_DISPLAY();
                    break;
                case enumDisplayOptions.DRONE_DISPLAY:
                    DRONE_DISPLAY();
                    break;
                case enumDisplayOptions.CUSTOMER_DISPLAY:
                    CUSTOMER_DISPLAY();
                    break;
                case enumDisplayOptions.PARCEL_DISPLAY:
                    PARCEL_DISPLAY();
                    break;
                case enumDisplayOptions.EXIT:
                    return;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }
        }
    }
}
