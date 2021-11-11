using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace BL
{
    public class BL
    {
        List<DroneToList> droneToList;
        IDAL.IDal dal;
        internal static double available;
        internal static double easy;
        internal static double medium;
        internal static double Heavy;
        internal static double ChargingRate;
        public BL()
        {
            droneToList = new();
            dal = new Dal.DalObject();

            double[] power = dal.PowerConsumptionRate();
            available = power[0];
            easy = power[1];
            medium = power[2];
            Heavy = power[3];
            ChargingRate = power[4];//אולי להוציא לפונ' נפרדת.
            List<IDAL.DO.Customer> customers = dal.DisplaysIistOfCustomers().ToList();
            List<IDAL.DO.Drone> drones=dal.DisplaysTheListOfDrons().ToList();
            for (int i = 0; i < drones.Count; i++)
            {
                droneToList[i].Id = drones[i].Id;
                droneToList[i].Model = drones[i].Model;
                droneToList[i].MaxWeight = drones[i].MaxWeight;
            }
            List<IDAL.DO.Parcel> PackagesInDelivery = dal.DisplaysIistOfparcels(i => (i.DroneId != 0 && i.Delivered == DateTime.MinValue)).ToList();
            for (int i = 0; i < PackagesInDelivery.Count; i++)
            {
                for (int j = 0; j < droneToList.Count; j++)
                {
                    if (droneToList[j].Id==PackagesInDelivery[i].DroneId)
                    {
                        droneToList[j].DroneStatuses = DroneStatuses.busy;
                        if (PackagesInDelivery[i].PickedUp==DateTime.MinValue)
                        {
                            droneToList[j].Location = TheNearestStation(PackagesInDelivery[i].SenderId);// יש לייצר את הפונקציה של חיפוש תחנה קרובה.
                            //The nearest station
                        }
                        else
                        {
                            IDAL.DO.Customer customer = customers.Find(customer => customer.Id == PackagesInDelivery[i].SenderId);
                            droneToList[j].Location.longitude = customer.longitude;
                            droneToList[j].Location.latitude = customer.lattitude;

                        }
                    }
                }
            }
        }
        
        private Location TheNearestStation(int sanderId)
        {
            List < IDAL.DO.Station> 
            return Location;
        }

        //public bool AddStation(Station station)
        //{
        //    //Station myStation = new()
        //    //{
        //    //    Id = station.Id,
        //    //    name = station.name,
        //    //    //longitude = longitude,
        //    //    //lattitude = lattitude,
        //    //    freeChargeSlots = station.freeChargeSlots
        //    //};
        //    bool test = dal.addStation(station);
        //    if (test)
        //        Console.WriteLine("the transaction completed successfully");
        //    else
        //        Console.WriteLine("ERROR");


        //    // int find = dalProgram.stations.FindIndex(Station => Station.Id == station.Id);
        //    return false;
        //}



    }

}
