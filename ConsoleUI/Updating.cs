using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using Dal;

namespace ConsoleUI
{
    class Updating
    {
        public enum enumUpdatingOptions { EXIT = 0, PACKAGE_ASSOCIATION, PACKAGE_COLLECTION, PACKAGE_DELIVERY, CARGING_DRONE, RELEASE_DRONE };
        public static void UpdatingOptions()
        {
            Console.WriteLine("enter 1 to  assigning a package to a drone");
            Console.WriteLine("enter 2 to  collection of a package by drone");
            Console.WriteLine("enter 3 to  delivery package to customer");
            Console.WriteLine("enter 4 to  sending a drone for charging at a base station");
            Console.WriteLine("enter 5 to  release drone from charging at base station");
            Console.WriteLine("enter 0 to EXIT");

            enumUpdatingOptions enumUpdating;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            enumUpdating = (enumUpdatingOptions)choice;
            switch (enumUpdating)
            {
                case enumUpdatingOptions.PACKAGE_ASSOCIATION:
                    PACKAGE_ASSOCIATION();
                    break;
                case enumUpdatingOptions.PACKAGE_COLLECTION:
                    PACKAGE_COLLECTION();
                    break;
                case enumUpdatingOptions.PACKAGE_DELIVERY:
                    PACKAGE_DELIVERY();
                    break;
                case enumUpdatingOptions.CARGING_DRONE:
                    CARGING_DRONE();
                    break;
                case enumUpdatingOptions.RELEASE_DRONE:
                    RELEASE_DRONE();
                    break;
                case enumUpdatingOptions.EXIT:
                    return;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void PACKAGE_ASSOCIATION()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool test = Dal.DalObject.AssignPackageToDrone(IdParcel, IdDrone);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// 
        /// </summary>
        public static void PACKAGE_COLLECTION()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool testParcel = Dal.DalObject.PackageCollectionByDrone(IdParcel);
            bool testDrone = Dal.DalObject.makeBusyTheDrone(IdDrone);
            if (testParcel && testDrone)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// 
        /// </summary>
        public static void PACKAGE_DELIVERY()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool testParcel = Dal.DalObject.DeliveryPackageToCustomer(IdParcel);
            bool testDrone = Dal.DalObject.makeAvailableTheDrone(IdDrone);
            if (testParcel && testDrone)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// 
        /// </summary>
        public static void CARGING_DRONE()
        {
            int num;
            Console.WriteLine("A list of all the stations where there is free space for charging will now be displayed." +
                " Select a station from the list according to ID number");
            do
            {
                Console.WriteLine("Press zero to display the list");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 0);

            ListView.STATIONS_WITH_AVAILABLE_CHARGING_SLOTS();

            Console.WriteLine("enter ID number of the ststion");
            int IdStation;
            int.TryParse(Console.ReadLine(), out IdStation);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool testStation = Dal.DalObject.reductionCargeSlotsToStation(IdStation);
            bool testDrone = Dal.DalObject.makeMaintenanceTheDrone(IdDrone);

            if (testDrone && testStation)
            {
                DroneCarge droneCarge = new()
                {
                    DroneID = IdDrone,
                    StationId = IdStation
                };
                Dal.DalObject.addDroneCarge(droneCarge);
                Console.WriteLine("the transaction completed successfully");
            }
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// 
        /// </summary>
        public static void RELEASE_DRONE()
        {
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);
            Console.WriteLine("enter ID number of the ststion");
            int IdStation;
            int.TryParse(Console.ReadLine(), out IdStation);
            bool testStation = Dal.DalObject.addingCargeSlotsToStation(IdStation);
            bool testDroneCarge = Dal.DalObject.ReleaseDroneCarge(IdDrone);
            bool testDrone = Dal.DalObject.makeAvailableTheDrone(IdDrone);
            if (testDroneCarge && testDrone && testStation)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
    }
}
