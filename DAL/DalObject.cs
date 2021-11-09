using IDAL.DO;
using System;
using System.Collections;
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
            if (find == -1)
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
            if (find == -1)
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
        public static void addDroneCarge(DroneCarge droneCarge)
        {
            DataSource.droneCarges.Add(droneCarge);
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
            int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);

            if (parcelFindIndex != -1 && droneFindIndex != -1)
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
        public static bool PackageCollectionByDrone(int parcelId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.PickedUp = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// this funcation makes the drone busy when the parcl Collection by drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        //public static bool makeBusyTheDrone(int droneId)
        //{
        //    Drone droneFind = DataSource.drones.Find(Drone => Drone.Id == droneId);
        //    int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);
        //    if (droneFindIndex != -1)
        //    {
        //        droneFind.Status = DroneStatuses.busy;
        //        DataSource.drones[droneFindIndex] = droneFind;
        //        return true;
        //    }
        //    else
        //        return false;
        //}
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public static bool DeliveryPackageToCustomer(int parcelId)
        {
            Parcel parcelFind = DataSource.parcels.Find(Parcel => Parcel.Id == parcelId);
            int parcelFindIndex = DataSource.parcels.FindIndex(Parcel => Parcel.Id == parcelId);

            if (parcelFindIndex != -1)
            {
                parcelFind.Delivered = DateTime.Now;
                DataSource.parcels[parcelFindIndex] = parcelFind;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// this funcation makes the drone available when the parcl delivered to the customer and more.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        //public static bool makeAvailableTheDrone(int droneId)
        //{
        //    Drone droneFind = DataSource.drones.Find(Drone => Drone.Id == droneId);
        //    int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);
        //    if (droneFindIndex != -1)
        //    {
        //        droneFind.Status = DroneStatuses.available;
        //        DataSource.drones[droneFindIndex] = droneFind;
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// this funcation makes the drone maintenance when the parcl Collection by drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        //public static bool makeMaintenanceTheDrone(int droneId)
        //{
        //    Drone droneFind = DataSource.drones.Find(Drone => Drone.Id == droneId);
        //    int droneFindIndex = DataSource.drones.FindIndex(Drone => Drone.Id == droneId);
        //    if (droneFindIndex != -1)
        //    {
        //        droneFind.Status = DroneStatuses.maintenance;
        //        DataSource.drones[droneFindIndex] = droneFind;
        //        return true;
        //    }
        //    else
        //        return false;
        //}
        /// <summary>
        /// This function remove object from list when drone release from charging .
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public static bool ReleaseDroneCarge(int droneId)
        {
            int indexDrone = DataSource.droneCarges.FindIndex(DroneCarge => DroneCarge.DroneID == droneId);//
            if (indexDrone != -1)
            {
                DataSource.droneCarges.RemoveAt(indexDrone);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function performs adding a charging slot to the station  
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public static bool addingCargeSlotsToStation(int stationId)
        {
            Station stationFind = DataSource.stations.Find(Station => Station.Id == stationId);
            int stationFindIndex = DataSource.stations.FindIndex(Station => Station.Id == stationId);

            if (stationFindIndex != -1)
            {
                stationFind.freeChargeSlots++;
                DataSource.stations[stationFindIndex] = stationFind;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function performs reduction  carge slot to the station. 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public static bool reductionCargeSlotsToStation(int stationId)
        {
            Station stationFind = DataSource.stations.Find(Station => Station.Id == stationId);
            int stationFindIndex = DataSource.stations.FindIndex(Station => Station.Id == stationId);

            if (stationFindIndex != -1)
            {
                stationFind.freeChargeSlots--;
                DataSource.stations[stationFindIndex] = stationFind;
                return true;
            }
            else
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
        /// This function transmits data of all existing stations.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Station> DisplaysIistOfStations(Predicate<Station> p = null)
        {
            return DataSource.stations.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing drones.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Drone> DisplaysTheListOfDrons(Predicate<Drone> p = null)
        {
            return DataSource.drones.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing customers.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Customer> DisplaysIistOfCustomers(Predicate<Customer> p = null)
        {
            return DataSource.customers.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing parcels.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Parcel> DisplaysIistOfparcels(Predicate<Parcel> p = null)
        {
            return DataSource.parcels.Where(d => p == null ? true : p(d)).ToList();
        }
        public static double[] PowerConsumptionRate()
        {
            double[] powerConsumptionRate = new double[] {
                DataSource.Config.available, DataSource.Config.easy,
                DataSource.Config.medium, DataSource.Config.Heavy,
                DataSource.Config.ChargingRate
            };

            return powerConsumptionRate;
        }

        public static bool removeDrone(int id)
        {
            int find = DataSource.drones.FindIndex(Drone => Drone.Id == id);
            if (find != -1)
            {
                DataSource.drones.RemoveAt(find);
                return true;
            }
            return false;
        }
        public static bool removeStation(int id)
        {
            int find = DataSource.stations.FindIndex(Station => Station.Id == id);
            if (find != -1)
            {
                DataSource.stations.RemoveAt(find);
                return true;
            }
            return false;
        }
        public static bool removeCustomer(int id)
        {
            int find = DataSource.customers.FindIndex(Customer => Customer.Id == id);
            if (find != -1)
            {
                DataSource.customers.RemoveAt(find);
                return true;
            }
            return false;
        }
        public static bool removeParcel(int id)
        {
            int find = DataSource.parcels.FindIndex(Parcel => Parcel.Id == id);
            if (find != -1)
            {
                DataSource.parcels.RemoveAt(find);
                return true;
            }
            return false;
        }
    }
}
