using IDAL.DO;
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
        public bool addStation(Station station)
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
        public bool addDrone(Drone drone)
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
        public bool addCustomer(Customer customer)
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
        public int addParsel(Parcel parcel)
        {
            DataSource.parcels.Add(parcel);
            return (DataSource.Config.ParcelIdRun++);
        }
        /// <summary>
        /// This function assigns a package to the drone.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="drone"></param>
        public void AssignPackageToDrone(Parcel parcel, Drone drone)
        {
            parcel.DroneId = drone.Id;
            parcel.Scheduled = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="drone"></param>
        public void PackageCollectionByDrone(Parcel parcel, Drone drone)
        {
            drone.Status = DroneStatuses.busy;
            parcel.PickedUp = DateTime.Now;
        }

        public void DeliveryPackageToCustomer(Parcel parcel, Drone drone)
        {
            parcel.Delivered = DateTime.Now;
            drone.Status = DroneStatuses.available;
        }
        public void SendingDroneForCharging(Drone drone, Station station)//יש לזכור במיין להוסיף זימון להצגת תחנות פנויות
        {
            drone.Status = DroneStatuses.maintenance;
            DataSource.droneCarges.Add(new DroneCarge() { DroneID = drone.Id, StationId = station.Id });
        }
        public bool ReleaseDroneFromCharging(Drone drone)
        {
            int index = DataSource.droneCarges.FindIndex(DroneCarge => DroneCarge.DroneID == drone.Id);
            if (index != -1)
            {
                int stationIdToRelease = DataSource.droneCarges[index].StationId;
                int indexStation = DataSource.stations.FindIndex(Station => Station.Id == stationIdToRelease);
                Station tempStation = DataSource.stations.Find(Station => Station.Id == stationIdToRelease);
                tempStation.ChargeSlots++;
                DataSource.stations[indexStation] = tempStation;
                DataSource.droneCarges.RemoveAt(index);
                drone.Status = DroneStatuses.available;
                drone.battery = 100.0;
                return true;
            }
            return false;
        }
        public Station? getStation(int Id)
        {
            Station? getStation = DataSource.stations.Find(Station => Station.Id == Id);
            return getStation != null ? getStation : null;
        }

        public Drone? getDrone(int Id)
        {
            Drone? getDrone = DataSource.drones.Find(Drone => Drone.Id == Id);
            return getDrone != null ? getDrone : null;
        }

        public Customer? getCustomer(int Id)
        {
            Customer? getCustomer = DataSource.customers.Find(Customer => Customer.Id == Id);
            return getCustomer != null ? getCustomer : null;
        }

        public Parcel? getParcel(int Id)
        {
            Parcel? getParcel = DataSource.parcels.Find(Parcel => Parcel.Id == Id);
            return getParcel != null ? getParcel : null;
        }
        public static List<T> getListTemplte<T>(List<T> ts) where T : struct
        {
            List<T> temp = new List<T>();
            for (int i = 0; i < ts.Count; i++)
            {
                temp.Add(ts[i]);
            }
            return temp;
        }

        public List<Station> DisplaysIistOfBaseStations()
        {
            return getListTemplte<Station>(DataSource.stations);
        }

        public List<Drone> DisplaysTheListOfDrons()
        {
            return getListTemplte<Drone>(DataSource.drones);
        }

        public List<Customer> DisplaysIistOfCustomers()
        {
            return getListTemplte<Customer>(DataSource.customers);
        }

        public List<Parcel> DisplaysIistOfparcels()
        {
            return getListTemplte<Parcel>(DataSource.parcels);
        }

        public List<Parcel> GetUnassignedPackages()
        {
            List<Parcel> UnassignedPackages = new List<Parcel>();

            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                UnassignedPackages.Add(DataSource.parcels.Find(Parcel => Parcel.DroneId == 0));
            }
            return UnassignedPackages;
        }

        public List<Station> baseStationsWithAvailableChargingStations()
        {
            List<Station> freeChargingSlots = new List<Station>();

            for (int i = 0; i < DataSource.stations.Count; i++)
            {
                freeChargingSlots.Add(DataSource.stations.Find(Station => Station.ChargeSlots > 0));
            }
            return freeChargingSlots;
        }
    }
}
