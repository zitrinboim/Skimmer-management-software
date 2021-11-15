using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class Updating
    {
        BL.BL BLProgram;
        public Updating(BL.BL _bLProgram)
        {
            BLProgram = _bLProgram;
        }
        public enum enumUpdatingOptions { EXIT = 0, UPDATE_NAME_OF_DRONE,STATION_DATA, PACKAGE_DELIVERY, CARGING_DRONE, RELEASE_DRONE };
        public void UpdatingOptions()
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
                case enumUpdatingOptions.UPDATE_NAME_OF_DRONE:
                    UpdateNameTheDrone();
                    break;
                case enumUpdatingOptions.STATION_DATA:
                    StationData(); 
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
        public void UpdateNameTheDrone()
        {
            Console.WriteLine("Enter the ID of the drone you want to change its name to");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);
            Console.WriteLine("Enter the name you want to change");
            string newName = Console.ReadLine();
            bool test = BLProgram.updateNameTheDrone(newName, IdDrone);
            if (test)
                Console.WriteLine("Updated successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// This function performs an update on packet collection by drone.
        /// </summary>
        public void StationData()
        {
            Console.WriteLine("Enter the ID of a station you would like to update");
            int Idstation;
            int.TryParse(Console.ReadLine(), out Idstation);
            Console.WriteLine("Enter a new name for the station, if you do not want to update Enter X");
            string newName = Console.ReadLine();
            Console.WriteLine("Update the total amount of charging stations, if you do not want to update Enter 0");
            int ChargingSlots;
            int.TryParse(Console.ReadLine(), out ChargingSlots);
            bool testParcel = BLProgram.updateStationData(Idstation, newName, ChargingSlots);
            if (testParcel)
                Console.WriteLine("the station data has been updated successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        public void PACKAGE_DELIVERY()
        {
            Console.WriteLine("enter ID number of the parcel");
            int IdParcel;
            int.TryParse(Console.ReadLine(), out IdParcel);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool testParcel = dalProgram.DeliveryPackageToCustomer(IdParcel);
            //bool testDrone = Dal.DalObject.makeAvailableTheDrone(IdDrone);
            if (testParcel /*&& testDrone*/)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// The function performs an update on sending a drone for charging.
        /// </summary>
        public void CARGING_DRONE()
        {

            //These lines invite a function that displays the stations that have free space for charging.
            ListView l = new();
            int num;
            Console.WriteLine("A list of all the stations where there is free space for charging will now be displayed." +
                " Select a station from the list according to ID number");
            do
            {
                Console.WriteLine("Press zero to display the list");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 0);

            l.STATIONS_WITH_AVAILABLE_CHARGING_SLOTS();

            Console.WriteLine("enter ID number of the ststion");
            int IdStation;
            int.TryParse(Console.ReadLine(), out IdStation);
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);

            bool testStation = dalProgram.reductionCargeSlotsToStation(IdStation);
            //bool testDrone = Dal.DalObject.makeMaintenanceTheDrone(IdDrone);

            if (/*testDrone &&*/ testStation)
            {
                DroneCarge droneCarge = new()
                {
                    DroneID = IdDrone,
                    StationId = IdStation
                };
                dalProgram.addDroneCarge(droneCarge);
                Console.WriteLine("the transaction completed successfully");
            }
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// The function performs an update on drone release from charging.
        /// </summary>
        public void RELEASE_DRONE()
        {
            Console.WriteLine("enter ID number of the drone");
            int IdDrone;
            int.TryParse(Console.ReadLine(), out IdDrone);
            Console.WriteLine("enter ID number of the ststion");
            int IdStation;
            int.TryParse(Console.ReadLine(), out IdStation);
            bool testStation = dalProgram.addingCargeSlotsToStation(IdStation);
            bool testDroneCarge = dalProgram.ReleaseDroneCarge(IdDrone);
            //bool testDrone = Dal.DalObject.makeAvailableTheDrone(IdDrone);
            if (testDroneCarge /*&& testDrone && testStation*/)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
    }
}
