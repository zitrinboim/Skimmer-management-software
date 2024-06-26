﻿using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{

    partial class BL : IBL
    {
        static internal BL instatnce = new BL();

        List<DroneToList> droneToLists;
        DalApi.IDal dal;
        DistanceAlgorithm distanceAlgorithm;

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

        /// <summary>
        /// c-tor
        /// </summary>
        private BL()
        {
            try
            {
                dal = DalApi.DalFactory.GetDal("DalXml");
                droneToLists = new();
                distanceAlgorithm = new();
                double battryOfDelivery;
                random = new Random(DateTime.Now.Millisecond);

                double[] power = dal.PowerConsumptionRate();
                available = power[0];
                easy = power[1];
                medium = power[2];
                Heavy = power[3];
                ChargingRate = power[4];

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
                        drone.parcelNumber = PackagesInDelivery[find].Id;

                        DO.Customer sander = customers.Find(customer => customer.Id == PackagesInDelivery[find].SenderId);
                        DO.Customer target = customers.Find(customer => customer.Id == PackagesInDelivery[find].TargetId);
                        Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };
                        Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                        if (PackagesInDelivery[find].PickedUp == DateTime.MinValue)
                        {
                            drone.Location = TheLocationForTheNearestStation(sanderLocation, stations);
                            battryOfDelivery = (distanceAlgorithm.DistanceBetweenPlaces(drone.Location, sanderLocation) * available)
                            + (distanceAlgorithm.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])
                            + (distanceAlgorithm.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);
                            drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                        else
                        {
                            drone.Location = sanderLocation;
                            battryOfDelivery = (distanceAlgorithm.DistanceBetweenPlaces(sanderLocation, targetLocation) *
                                power[(int)PackagesInDelivery[find].weight])
                            + (distanceAlgorithm.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);
                            drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                        }
                    }
                    else
                    {
                        int index;
                        DO.DroneCarge droneCarge = new();
                        try
                        {
                            droneCarge = dal.getDroneCargeByDroneId(drone.Id);
                        }
                        catch (DO.IdNotExistExeptions Ex)
                        {
                            droneCarge.DroneID = 0;
                        }
                        if (droneCarge.DroneID == 0)
                            drone.DroneStatuses = DroneStatuses.available;
                        else
                        {
                            drone.DroneStatuses = DroneStatuses.maintenance;
                            DO.Station station = dal.getStation(droneCarge.StationId);
                            drone.Location = new() { longitude = station.longitude, latitude = station.lattitude };
                            drone.battery = random.NextDouble() * 20.0;
                        }

                        if (drone.DroneStatuses == DroneStatuses.available)
                        {
                            List<DO.Parcel> droneParcels = PackagesInDelivery.FindAll(i => i.Delivered != DateTime.MinValue);

                            index = random.Next(0, droneParcels.Count);
                            if (droneParcels.Count > 0)
                            {
                                DO.Customer target = customers.Find(customer => customer.Id == droneParcels[index].TargetId);
                                Location location = new() { latitude = target.lattitude, longitude = target.longitude };
                                drone.Location = location;
                            }

                            battryOfDelivery = distanceAlgorithm.DistanceBetweenPlaces(drone.Location, TheLocationForTheNearestStation(drone.Location, stations)) * available;
                            drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + 30.0;
                            if (drone.battery > 100.0)
                                drone.battery = 100.0;
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
        /// <summary>
        /// This function brings up the password.
        /// </summary>
        /// <returns></returns>
        public int Password(){ return dal.PasswordDL(); }
    }
}

