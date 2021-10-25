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
        public enum enumPisplayOptions { EXIT = 0, STATION_DISPLAY, DRONE_DISPLAY, CUSTOMER_DISPLAY, PARCEL_DISPLAY, };
        public static void PisplayOptions()
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
        /// 
        /// </summary>
        public static void STATION_DISPLAY()
        {
            Console.WriteLine("enter ID number of the ststion");
            int IdStation;
            int.TryParse(Console.ReadLine(), out IdStation);
            Station station = (Station)DalObject.getStation(IdStation);
            if (station.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
                Console.WriteLine(station.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        public static void DRONE_DISPLAY()
        {
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);
            Drone drone = (Drone)DalObject.getDrone(IdDrone);
            if (drone.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
                Console.WriteLine(drone.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        public static void CUSTOMER_DISPLAY()
        {
            Console.WriteLine("enter ID number of the customer");
            int IdCustomer;
            int.TryParse(Console.ReadLine(), out IdCustomer);
            Customer customer = (Customer)DalObject.getCustomer(IdCustomer);
            if (customer.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
                Console.WriteLine(customer.ToString());
        }
        ///
        public static void PARCEL_DISPLAY()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Parcel parcel = (Parcel)DalObject.getParcel(IdParcel);
            if (parcel.Id == 0)
                Console.WriteLine("There is no station with this ID number");
            else
                Console.WriteLine(parcel.ToString());
        }
    }
}
