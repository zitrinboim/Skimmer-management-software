using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using Dal;

namespace ConsoleUI
{
    class Pisplay
    {
        public DalObject dalProgram = new DalObject();
        public enum enumPisplayOptions { EXIT = 0, STATION_DISPLAY, DRONE_DISPLAY, CUSTOMER_DISPLAY, PARCEL_DISPLAY, };
        public void DisplayOptions()
        {
            Console.WriteLine("enter 1 to  get the information on station");
            Console.WriteLine("enter 2 to  get the information on drone");
            Console.WriteLine("enter 3 to  get the information on customer");
            Console.WriteLine("enter 4 to  get the information on parcel");
            Console.WriteLine("enter 0 to EXIT");

            enumPisplayOptions enumUpdating;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            enumUpdating = (enumPisplayOptions)choice;
            switch (enumUpdating)
            {
                case enumPisplayOptions.STATION_DISPLAY:
                    STATION_DISPLAY();
                    break;
                case enumPisplayOptions.DRONE_DISPLAY:
                    DRONE_DISPLAY();
                    break;
                case enumPisplayOptions.CUSTOMER_DISPLAY:
                    CUSTOMER_DISPLAY();
                    break;
                case enumPisplayOptions.PARCEL_DISPLAY:
                    PARCEL_DISPLAY();
                    break;
                case enumPisplayOptions.EXIT:
                    return;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }
        }
        /// <summary>
        /// This function converts coordinate from decimal display to display using minute and second degrees.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></string>
        public string ConvertDecimalToDegMinSec(double value)
        {
            int deg = (int)value;
            value = Math.Abs(value - deg);
            int min = (int)(value * 60);
            value = value - (double)min / 60;
            int sec = (int)(value * 3600);
            value = value - (double)sec / 3600;
            return deg.ToString() + '°' + min.ToString() + "'" + sec.ToString() + "''";
        }
        /// <summary>
        /// The function provides information about a requested station.
        /// </summary>
        public void STATION_DISPLAY()
        {
            ListView l = new();
            Console.WriteLine("enter ID number of the ststion");
            int IdStation;
            int.TryParse(Console.ReadLine(), out IdStation);
            Station station = (Station)dalProgram.getStation(IdStation);
            if (station.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
            {
                Console.WriteLine(station + l.toString1(station.longitude, station.lattitude));
            }
        }
        /// <summary>
        /// The function provides information about a requested drone
        /// </summary>
        public void DRONE_DISPLAY()
        {
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);
            Drone drone = (Drone)dalProgram.getDrone(IdDrone);
            if (drone.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
                Console.WriteLine(drone);
        }
        /// <summary>
        /// The function provides information about a requested customer.
        /// </summary>
        public void CUSTOMER_DISPLAY()
        {
            Console.WriteLine("enter ID number of the customer");
            int IdCustomer;
            int.TryParse(Console.ReadLine(), out IdCustomer);
            Customer customer = (Customer)dalProgram.getCustomer(IdCustomer);
            if (customer.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
            {
                Console.WriteLine(string.Format("Customer\nID {0}\tname {1}\tphone" +
                    " {2}\tlongitude {3}N\tlattitude {4}E", customer.Id, customer.name,
                    customer.phone, ConvertDecimalToDegMinSec(customer.longitude),
                    ConvertDecimalToDegMinSec(customer.lattitude)));
            }
        }
        /// <summary>
        /// The function provides information about a requested parcel
        /// </summary>
        public void PARCEL_DISPLAY()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Parcel parcel = (Parcel)dalProgram.getParcel(IdParcel);
            if (parcel.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
                Console.WriteLine(parcel);
        }
    }
}
