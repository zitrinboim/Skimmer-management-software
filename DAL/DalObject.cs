﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalObject
    {/// <summary>
     /// This function activates the constructor of Initialize, so that all the Lists of the various entities are initialized.
     /// </summary>
        public DalObject() => DataSource.Initialize();
        /// <summary>
        /// This function allows the user to add a base station to the list.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></true/false>
        public static bool addStation(Station station)
        {
            int find = DataSource.stations.FindIndex(Station => Station.Id == station.Id);
            //Safety mechanism to prevent the overrun of an existing entity with the same ID.
            if (find <= 0)
            {
                DataSource.stations.Add(station);
                return true;
            }
            return false;
        }
        /// <summary>
        /// This function allows the user to add a drone to the list.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        public static bool addDrone(Drone drone)
        {
            int find = DataSource.drones.FindIndex(Drone => Drone.Id == drone.Id);
            //Safety mechanism to prevent the overrun of an existing entity with the same ID.
            if (find <= 0)
            {
                DataSource.drones.Add(drone);
                return true;
            }
            return false;
        }
        /// <summary>
        /// This function allows the user to add a customer to the list.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool addCustomer(Customer customer)
        {
            int find = DataSource.drones.FindIndex(Customer => Customer.Id == customer.Id);
            //Safety mechanism to prevent the overrun of an existing entity with the same ID.
            if (find <= 0)
            {
                DataSource.customers.Add(customer);
                return true;
            }
            return false;
        }
        /// <summary>
        ///This function allows the user to add a parcel to the list.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public static int addParsel(Parcel parcel)
        {
            parcel.Id = DataSource.Config.ParcelIdRun;
            DataSource.parcels.Add(parcel);
            return (DataSource.Config.ParcelIdRun++);
        }
        /// <summary>
        /// This function assigns a package to the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public static bool AssignPackageToDrone(int parcelId, int droneId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.DroneId = droneId;
                parcelFind.Scheduled = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function performs an update on packet collection by drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public static bool PackageCollectionByDrone(int parcelId, int droneId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);
            Drone droneFind = DataSource.drones.Find(Drone => Drone.Id == droneId);
            int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);

            if (parcelFindIndex != -1 && droneFindIndex != -1)
            {
                parcelFind.PickedUp = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
                droneFind.Status = DroneStatuses.busy;
                DataSource.drones[droneFindIndex] = droneFind;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public static bool DeliveryPackageToCustomer(int parcelId, int droneId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);
            Drone droneFind = DataSource.drones.Find(Drone => Drone.Id == droneId);
            int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);

            if (parcelFindIndex != -1 && droneFindIndex != -1)
            {
                parcelFind.Delivered = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
                droneFind.Status = DroneStatuses.available;
                DataSource.drones[droneFindIndex] = droneFind;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function performs an update of sending a drone for charging.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public static bool SendingDroneForCharging(int droneId, int stationId)//יש לזכור במיין להוסיף זימון להצגת תחנות פנויות
        {

            Drone droneFind = DataSource.drones.Find(Drone => Drone.Id == droneId);
            int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);

            if (droneFindIndex != -1)
            {

                droneFind.Status = DroneStatuses.maintenance;
                DataSource.drones[droneFindIndex] = droneFind;
                DataSource.droneCarges.Add(new DroneCarge() { DroneID = droneId, StationId = stationId });//Create a new show from a class DroneCarge. 
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function updates the release of drone from charging.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public static bool ReleaseDroneFromCharging(int droneId)
        {
            int indexDrone = DataSource.droneCarges.FindIndex(DroneCarge => DroneCarge.DroneID == droneId);//
            if (indexDrone != -1)
            {
                //These lines add a free charging slot at the station where the charging was.
                int stationIdToRelease = DataSource.droneCarges[indexDrone].StationId;
                int indexStation = DataSource.stations.FindIndex(Station => Station.Id == stationIdToRelease);
                Station tempStation = DataSource.stations.Find(Station => Station.Id == stationIdToRelease);
                tempStation.ChargeSlots++;
                DataSource.stations[indexStation] = tempStation;

                DataSource.droneCarges.RemoveAt(indexDrone);//Remove object from list.

                //These lines Updates the status and battery after recharging.
                Drone tempDrone = DataSource.drones.Find(Drone => Drone.Id == droneId);
                tempDrone.Status = DroneStatuses.available;
                tempDrone.battery = 100.0;
                DataSource.drones[indexDrone] = tempDrone;

                return true;
            }
            return false;
        }
        /// <summary>
        /// This function transmits the data of the requested station according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Station? getStation(int Id)
        {
            Station? getStation = DataSource.stations.Find(Station => Station.Id == Id);
            return getStation != null ? getStation : null;
        }
        /// <summary>
        /// This function transmits the requested drone data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Drone? getDrone(int Id)
        {
            Drone? getDrone = DataSource.drones.Find(Drone => Drone.Id == Id);
            return getDrone != null ? getDrone : null;
        }
        /// <summary>
        /// This function transmits the requested customer data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Customer? getCustomer(int Id)
        {
            Customer? getCustomer = DataSource.customers.Find(Customer => Customer.Id == Id);
            return getCustomer != null ? getCustomer : null;
        }
        /// <summary>
        /// This function transmits the requested parcel data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Parcel? getParcel(int Id)
        {
            Parcel? getParcel = DataSource.parcels.Find(Parcel => Parcel.Id == Id);
            return getParcel != null ? getParcel : null;
        }
        /// <summary>
        /// This function serves as a template for data transfer of complete lists of any entity that be required.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static List<T> getListTemplte<T>(List<T> ts) where T : struct
        {
            List<T> temp = new List<T>();
            for (int i = 0; i < ts.Count; i++)
            {
                temp.Add(ts[i]);
            }
            return temp;
        }
        /// <summary>
        /// This function transmits data of all existing stations.
        /// </summary>
        /// <returns></returns>
        public static List<Station> DisplaysIistOfStations()
        {
            return getListTemplte<Station>(DataSource.stations);
        }
        /// <summary>
        /// This function transmits data of all existing drones.
        /// </summary>
        /// <returns></returns>
        public static List<Drone> DisplaysTheListOfDrons()
        {
            return getListTemplte<Drone>(DataSource.drones);
        }
        /// <summary>
        /// This function transmits data of all existing customers.
        /// </summary>
        /// <returns></returns>
        public static List<Customer> DisplaysIistOfCustomers()
        {
            return getListTemplte<Customer>(DataSource.customers);
        }
        /// <summary>
        /// This function transmits data of all existing parcels.
        /// </summary>
        /// <returns></returns>
        public static List<Parcel> DisplaysIistOfparcels()
        {
            return getListTemplte<Parcel>(DataSource.parcels);
        }
        /// <summary>
        /// This function transmits data of all packages not yet associated with the drone.
        /// </summary>
        /// <returns></returns>
        public static List<Parcel> GetUnassignedPackages()
        {
            List<Parcel> UnassignedPackages = new List<Parcel>();

            DataSource.parcels.ForEach(delegate (Parcel parcel)
            {
                if (parcel.DroneId == 0)
                    UnassignedPackages.Add(parcel);
            });
            return UnassignedPackages;
        }
        /// <summary>
        /// This function transmits data of all stations that have free charging slots.
        /// </summary>
        /// <returns></returns>
        public static List<Station> stationsWithFreeChargingSlots()
        {
            List<Station> freeChargingSlots = new List<Station>();

            DataSource.stations.ForEach(delegate (Station station)
            {
                if (station.ChargeSlots > 0)
                    freeChargingSlots.Add(station);
            });
            return freeChargingSlots;
        }
    }
}
