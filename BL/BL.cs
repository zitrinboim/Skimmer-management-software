using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    
    partial class BL : IBL
    {
        static internal BL instatnce=new BL();
        
        List<DroneToList> droneToLists;
        DalApi.IDal dal;
        DistanceAlgorithm d;
        Random random;
        List<DO.Station> stations;
        List<DO.Customer> customers;
        List<DO.Drone> drones;
        List<DO.Parcel> PackagesInDelivery;

        internal static double available;
        internal static double easy;
        internal static double medium;
        internal static double Heavy;
        internal static double ChargingRate;
        private BL()
        {
            try
            {
                dal = DalApi.DalFactory.GetDal("obj");
                droneToLists = new();
                d = new();
                double battryOfDelivery;
                random = new Random(DateTime.Now.Millisecond);

                double[] power = dal.PowerConsumptionRate();//לאתחל את הערכים של הצריכה 
                available = power[0];
                easy = power[1];
                medium = power[2];
                Heavy = power[3];
                ChargingRate = power[4];//אולי להוציא לפונ' נפרדת.

                stations = dal.DisplaysIistOfStations().ToList();
                customers = dal.DisplaysIistOfCustomers().ToList();
                drones = dal.DisplaysTheListOfDrons().ToList();
                PackagesInDelivery = dal.DisplaysIistOfparcels(i => i.Scheduled != DateTime.MinValue).ToList();
                foreach (DO.Drone item in drones)
                {
                    droneToLists.Add(new()
                    {
                        Id = item.Id,
                        Model = item.Model,
                        MaxWeight = (WeightCategories)item.MaxWeight
                    });
                }
                foreach (DroneToList drone in droneToLists)
                {
                    int find = PackagesInDelivery.FindIndex(i => i.DroneId == drone.Id && i.Delivered == DateTime.MinValue);
                    if (find != -1)
                    {
                        drone.DroneStatuses = DroneStatuses.busy;

                        DO.Customer sander = customers.Find(customer => customer.Id == PackagesInDelivery[find].SenderId);
                        DO.Customer target = customers.Find(customer => customer.Id == PackagesInDelivery[find].TargetId);
                        Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };
                        Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                        if (PackagesInDelivery[find].PickedUp == DateTime.MinValue)
                        {
                            drone.Location = TheLocationForTheNearestStation(sanderLocation, stations);
                            battryOfDelivery = (d.DistanceBetweenPlaces(drone.Location, sanderLocation) * available)
                            + (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])
                            + (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);
                            drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                        else
                        {
                            drone.Location = sanderLocation;
                            battryOfDelivery = (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])//להסביר
                            + (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);
                            drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                    }
                    else
                    {
                        int index;

                        drone.DroneStatuses = (DroneStatuses)random.Next(1, 3);
                        if (drone.DroneStatuses == (DroneStatuses)1)
                        {
                            List<DO.Parcel> droneParcels = PackagesInDelivery.FindAll(i => i.Delivered != DateTime.MinValue);

                            index = random.Next(0, droneParcels.Count);
                            DO.Customer target = customers.Find(customer => customer.Id == droneParcels[index].TargetId);
                            Location location = new() { latitude = target.lattitude, longitude = target.longitude };
                            drone.Location = location;

                            battryOfDelivery = d.DistanceBetweenPlaces(drone.Location, TheLocationForTheNearestStation(drone.Location, stations)) * available;
                            drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                        else
                        {
                            index = random.Next(0, stations.Count);
                            Location location = new() { latitude = stations[index].lattitude, longitude = stations[index].longitude };
                            drone.Location=location;
   
                            drone.battery = random.NextDouble() * 20.0;
                        }
                    }

                }
            }

            catch (DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
    }
}

