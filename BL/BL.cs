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
        List<DroneToList> droneToLists;
        IDAL.IDal dal;
        DistanceAlgorithm d;

        internal static double available;
        internal static double easy;
        internal static double medium;
        internal static double Heavy;
        internal static double ChargingRate;
        public BL()
        {
            droneToLists = new();
            d =  new();
            dal = new Dal.DalObject();
            Random random = new Random(DateTime.Now.Millisecond);
            double battryOfDelivery;

            double[] power = dal.PowerConsumptionRate();//לאתחל את הערכים של הצריכה 
            available = power[0];
            easy = power[1];
            medium = power[2];
            Heavy = power[3];
            ChargingRate = power[4];//אולי להוציא לפונ' נפרדת.

            List<IDAL.DO.Station> stations = dal.DisplaysIistOfStations().ToList();
            List<IDAL.DO.Customer> customers = dal.DisplaysIistOfCustomers().ToList();
            List<IDAL.DO.Drone> drones = dal.DisplaysTheListOfDrons().ToList();
            List<IDAL.DO.Parcel> PackagesInDelivery = dal.DisplaysIistOfparcels(i => i.DroneId != 0 ).ToList();

            for (int i = 0; i < drones.Count; i++)// לקחת פונ' מיהודה שור
            {
                droneToLists[i].Id = drones[i].Id;
                droneToLists[i].Model = drones[i].Model;
                droneToLists[i].MaxWeight = drones[i].MaxWeight;
            }

            foreach (DroneToList drone in droneToLists)
            {
                int find = PackagesInDelivery.FindIndex(i => i.DroneId == drone.Id && i.Delivered == DateTime.MinValue);
                if (find != -1)
                {
                    drone.DroneStatuses = DroneStatuses.busy;

                    IDAL.DO.Customer sander = customers.Find(customer => customer.Id == PackagesInDelivery[find].SenderId);
                    IDAL.DO.Customer target = customers.Find(customer => customer.Id == PackagesInDelivery[find].TargetId);
                    Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };
                    Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                    if (PackagesInDelivery[find].PickedUp == DateTime.MinValue)
                    {
                        drone.Location = TheNearestStation(sanderLocation, stations);
                        battryOfDelivery = (d.DistanceBetweenPlaces(drone.Location, sanderLocation) * available)
                        + (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])
                        + (d.DistanceBetweenPlaces(targetLocation, TheNearestStation(targetLocation, stations)) * available);
                        drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                    }
                    else
                    {
                        drone.Location = sanderLocation;
                        battryOfDelivery = (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])//להסביר
                        + (d.DistanceBetweenPlaces(targetLocation, TheNearestStation(targetLocation, stations)) * available);
                        drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                    }
                }
                else
                {
                    int index;

                    drone.DroneStatuses = (DroneStatuses)random.Next(1, 3);
                    if (drone.DroneStatuses == (DroneStatuses)1)
                    {
                        List<IDAL.DO.Parcel> droneParcels = PackagesInDelivery.FindAll(i => i.DroneId == drone.Id && i.Delivered != DateTime.MinValue);
                        index = random.Next(0, droneParcels.Count);
                        IDAL.DO.Customer target = customers.Find(customer => customer.Id == droneParcels[index].TargetId);
                        drone.Location.latitude = target.lattitude;
                        drone.Location.longitude = target.longitude;

                        battryOfDelivery = d.DistanceBetweenPlaces(drone.Location, TheNearestStation(drone.Location, stations)) * available;
                        drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                    }
                    else
                    {
                        index = random.Next(0, stations.Count);
                        drone.Location.latitude = stations[index].lattitude;
                        drone.Location.longitude = stations[index].longitude;
                        drone.battery = random.NextDouble() * 20.0;
                    }
                }
            }
        }
        private Location TheNearestStation(Location customerLocation, List<IDAL.DO.Station> stations)
        {
            IDAL.DO.Station closeStation = stations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < stations.Count; i++)
            {
                Location stationLocation = new() { latitude = stations[i].lattitude, longitude = stations[i].longitude };
                double distance1 = d.DistanceBetweenPlaces(stationLocation, customerLocation);
                if (distance1 < distance)
                {
                    distance = distance1;
                    closeStation = stations[i];
                }
            }
            Location location = new() { longitude = closeStation.longitude, latitude = closeStation.lattitude };
            return location;
        }
    }
}
