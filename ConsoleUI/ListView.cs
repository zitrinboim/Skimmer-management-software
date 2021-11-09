using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using Dal;

namespace ConsoleUI
{
    class ListView
    {
        public enum enumListViewOptions { EXIT = 0, LIST_STATIONS, LIST_DRONS, LIST_CUSTOMERS, LIST_PARCELS, PARCELS_HAVE_NOT_DRONS, STATIONS_WITH_AVAILABLE_CHARGING_SLOTS };
        public static void ListViewOptions()
        {
            Console.WriteLine("enter 1 to view the list of stations");
            Console.WriteLine("enter 2 to view the list of drons");
            Console.WriteLine("enter 3 to view the list of customers");
            Console.WriteLine("enter 4 to view the list of parcels");
            Console.WriteLine("enter 5 to view the list of parcels that do not have an associated drone");
            Console.WriteLine("enter 6 to view the list of stations that have free charging slots");
            Console.WriteLine("enter 0 to EXIT");

            enumListViewOptions enumUpdating;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            enumUpdating = (enumListViewOptions)choice;
            switch (enumUpdating)
            {
                case enumListViewOptions.LIST_STATIONS:
                    LIST_STATIONS();
                    break;
                case enumListViewOptions.LIST_DRONS:
                    LIST_DRONS();
                    break;
                case enumListViewOptions.LIST_CUSTOMERS:
                    LIST_CUSTOMERS();
                    break;
                case enumListViewOptions.LIST_PARCELS:
                    LIST_PARCELS();
                    break;
                case enumListViewOptions.PARCELS_HAVE_NOT_DRONS:
                    PARCELS_HAVE_NOT_DRONS();
                    break;
                case enumListViewOptions.STATIONS_WITH_AVAILABLE_CHARGING_SLOTS:
                    STATIONS_WITH_AVAILABLE_CHARGING_SLOTS();
                    break;
                case enumListViewOptions.EXIT:
                    return;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }
        }

        /// <summary>
        /// The function provides information on the entire list of stations.
        /// </summary>
        public static void LIST_STATIONS()
        {
            List<Station> listToPrint = DalObject.DisplaysIistOfStations().ToList();
            listToPrint.ForEach(i => Console.WriteLine(i + toString1(i.longitude,i.lattitude)));
        }

        public static string toString1(double i, double s)
        {
            return string.Format(" {0}\tlongitude {1}N\tlattitude ", Pisplay.ConvertDecimalToDegMinSec(i),
                                Pisplay.ConvertDecimalToDegMinSec(s));
        }

        /// <summary>
        /// The function provides information on the entire list of drons.
        /// </summary>
        public static void LIST_DRONS()
        {
            List<Drone> listToPrint = DalObject.DisplaysTheListOfDrons().ToList();
            listToPrint.ForEach(i=> Console.WriteLine(i));
        }

        /// <summary>
        /// The function provides information on the entire list of customers.
        /// </summary>
        public static void LIST_CUSTOMERS()
        {
            List<Customer> listToPrint = DalObject.DisplaysIistOfCustomers().ToList();
            listToPrint.ForEach(i => Console.WriteLine(i+toString1(i.longitude,i.lattitude)));
           
        }
        /// <summary>
        /// The function provides information on the entire list of parcels.
        /// </summary>
        public static void LIST_PARCELS()
        {
            List<Parcel> listToPrint = DalObject.DisplaysIistOfparcels().ToList();
            listToPrint.ForEach(i=> Console.WriteLine(i));
        }

        /// <summary>
        /// The function provides information on all parcels not associated with the drone.
        /// </summary>
        public static void PARCELS_HAVE_NOT_DRONS()
        {
            List<Parcel> listToPrint = DalObject.DisplaysIistOfparcels(i=>i.DroneId==0).ToList();
            if (listToPrint.Count() == 0)
            {
                Console.WriteLine("There are no packages in the requested status");
                return;
            }
            listToPrint.ForEach(i=> Console.WriteLine(i));
           
        }

        /// <summary>
        /// The function provides information on all stations that have free charging slots.
        /// </summary>
        public static void STATIONS_WITH_AVAILABLE_CHARGING_SLOTS()
        {
            List<Station> listToPrint = DalObject.DisplaysIistOfStations(i=>i.freeChargeSlots>0).ToList();
            listToPrint.ForEach(i=> Console.WriteLine(i));
        }

    }
}
