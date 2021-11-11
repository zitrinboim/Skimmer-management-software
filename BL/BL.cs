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
        DistanceAlgorithm d = new();

        internal static double available;
        internal static double easy;
        internal static double medium;
        internal static double Heavy;
        internal static double ChargingRate;
        public BL()
        {
            droneToList = new();
            dal = new Dal.DalObject();
            Random random = new Random(DateTime.Now.Millisecond);

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
            for (int i = 0; i < PackagesInDelivery.Count; i++)//האם יש דרך ליעל את הפעולה הזו
            {
                for (int j = 0; j < droneToList.Count; j++)
                {
                    if (droneToList[j].Id==PackagesInDelivery[i].DroneId)
                    {
                        droneToList[j].DroneStatuses = DroneStatuses.busy;

                        IDAL.DO.Customer sander = customers.Find(customer => customer.Id == PackagesInDelivery[i].SenderId);
                        IDAL.DO.Customer target = customers.Find(customer => customer.Id == PackagesInDelivery[i].TargetId);
                        Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };
                        Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };
                        double battryOfDelivery;
                        
                       
                        if (PackagesInDelivery[i].PickedUp==DateTime.MinValue)
                        {
                            droneToList[j].Location = TheNearestStation(sanderLocation);
                            battryOfDelivery =(d.DistanceBetweenPlaces(droneToList[j].Location,sanderLocation)*available)
                            +(d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[i].weight])
                            + (d.DistanceBetweenPlaces(targetLocation, TheNearestStation(targetLocation)) * available);
                            droneToList[j].battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                        else
                        {
                            droneToList[j].Location= sanderLocation;
                            battryOfDelivery = (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[i].weight])
                            + (d.DistanceBetweenPlaces(targetLocation, TheNearestStation(targetLocation)) * available);
                            droneToList[j].battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                    }
                }
            }
            foreach (DroneToList item in droneToList)
            {
                if (item.DroneStatuses != DroneStatuses.busy)
                    item.DroneStatuses = (DroneStatuses)random.Next(1,3);   
            }
            
        }
        
        internal Location TheNearestStation(Location customerLocation)
        {
            List<IDAL.DO.Station> stations = dal.DisplaysIistOfStations().ToList();
            IDAL.DO.Station closeStation = stations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < stations.Count; i++)
            {
                Location stationLocation = new() { latitude = stations[i].lattitude, longitude = stations[i].longitude };
                double distance1 = d.DistanceBetweenPlaces(stationLocation, customerLocation);
                if (distance1<distance)
                {
                    distance = distance1;
                    closeStation = stations[i];
                }
            }
            Location location = new() { longitude = closeStation.longitude, latitude = closeStation.lattitude };
            return location ;
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
