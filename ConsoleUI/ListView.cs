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
            Console.WriteLine("enter 1 to  to view the list of stations");
            Console.WriteLine("enter 2 to  to view the list of drons");
            Console.WriteLine("enter 3 to  to view the list of customers");
            Console.WriteLine("enter 4 to  to view the list of parcels");
            Console.WriteLine("enter 5 to  to view the list of parcels that do not have an associated drone");
            Console.WriteLine("enter 6 to  to view the list of stations that have free charging slots");
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
        public static void LIST_STATIONS()
        {
            List<Station> listToPrint = DalObject.DisplaysIistOfStations();
            listToPrint.ForEach(delegate (Station station)
            {
                station.ToString();
            });
        }
        public static void LIST_DRONS()
        {
            List<Drone> listToPrint = DalObject.DisplaysTheListOfDrons();
            listToPrint.ForEach(delegate (Drone drone)
            {
                drone.ToString();
            });
        }
        public static void LIST_CUSTOMERS()
        {
            List<Customer> listToPrint = DalObject.DisplaysIistOfCustomers();
            listToPrint.ForEach(delegate (Customer customer)
            {
                customer.ToString();
            });
        }
        public static void LIST_PARCELS()
        {
            List<Parcel> listToPrint = DalObject.DisplaysIistOfparcels();
            listToPrint.ForEach(delegate (Parcel parcel)
            {
                parcel.ToString();
            });
        }
        public static void PARCELS_HAVE_NOT_DRONS()
        {
            List<Parcel> listToPrint = DalObject.GetUnassignedPackages();
            listToPrint.ForEach(delegate (Parcel parcel)
            {
                parcel.ToString();
            });
        }
        public static void STATIONS_WITH_AVAILABLE_CHARGING_SLOTS()
        {
            List<Station> listToPrint = DalObject.stationsWithFreeChargingSlots();
            listToPrint.ForEach(delegate (Station station)
            {
                station.ToString();
            });
        }
    }
}
