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
            Console.WriteLine("enter 1 to  Assigning a package to a skimmer");
            Console.WriteLine("enter 2 to  Collection of a package by skimmer");
            Console.WriteLine("enter 3 to  Delivery package to customer");
            Console.WriteLine("enter 4 to  Sending a drone for charging at a base station");
            Console.WriteLine("enter 5 to  Release drone from charging at base station");
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
                case enumInsertOptions.EXIT:
                    break;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }

        }
        public static void PACKAGE_ASSOCIATION()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);
           
            bool test = Dal.DalObject.AssignPackageToDrone(IdParcel,IdDrone);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        public static void PACKAGE_COLLECTION()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool test = Dal.DalObject.PackageCollectionByDrone(IdParcel, IdDrone);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        public static void PACKAGE_DELIVERY()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool test = Dal.DalObject.DeliveryPackageToCustomer(IdParcel, IdDrone);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        public static void CARGING_DRONE()
        {

        }
    }
}
