using IDAL.DO;
using IDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalObject : IDal
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
            if (find == -1)
            {
                DataSource.stations.Add(station);
                return true;
            }
            else
            {
                throw new IdExistExeptions("Sorry, i have already a station with this id:" + station.Id);
            }
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
            if (find == -1)
            {
                DataSource.drones.Add(drone);
                return true;
            }
                throw new IdExistExeptions("Sorry, i have already a drone with this id:" + drone.Id);
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
                throw new IdExistExeptions("Sorry, i have already a customer with this id:" + customer.Id);
        }
        /// <summary>
        ///This function allows the user to add a parcel to the list.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public int addParsel(Parcel parcel)
        {
            parcel.Id = DataSource.Config.ParcelIdRun;
            DataSource.parcels.Add(parcel);
            return (DataSource.Config.ParcelIdRun++);
        }
        /// <summary>
        ///  This function add element to the list of drinecarge.
        /// </summary>
        /// <param name="droneCarge"></param>
        public void addDroneCarge(DroneCarge droneCarge)
        {
            DataSource.droneCarges.Add(droneCarge);
        }

        /// <summary>
        /// This function assigns a package to the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool AssignPackageToDrone(int parcelId, int droneId)
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
                throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function performs an update on packet collection by drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool PackageCollectionByDrone(int parcelId)
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
                throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool DeliveryPackageToCustomer(int parcelId)
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
                throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
        /// <summary>
        /// This function remove object from list when drone release from charging .
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool ReleaseDroneCarge(int droneId)
        {
            int indexDrone = DataSource.droneCarges.FindIndex(DroneCarge => DroneCarge.DroneID == droneId);//
            if (indexDrone != -1)
            {
                DataSource.droneCarges.RemoveAt(indexDrone);
                return true;
            }
            else
                throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function performs adding a charging slot to the station  
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public bool addingCargeSlotsToStation(int stationId)
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
                throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        /// <summary>
        /// This function performs reduction  carge slot to the station. 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public bool reductionCargeSlotsToStation(int stationId)
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
                throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function transmits the data of the requested station according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Station getStation(int Id)
        {
            Station getStation = DataSource.stations.Find(Station => Station.Id == Id);
            return getStation.Id!=default ? getStation : throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        public DroneCarge getDroneCargeByStationId(int stationId)
        {
            DroneCarge getDroneCarge = DataSource.droneCarges.Find(DroneCarge => DroneCarge.StationId == stationId);
            return getDroneCarge.StationId != default ? getDroneCarge : throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        public DroneCarge getDroneCargeByDroneId(int droneId)
        {
            DroneCarge getDroneCarge = DataSource.droneCarges.Find(DroneCarge => DroneCarge.DroneID == droneId);
            return getDroneCarge.StationId != default ? getDroneCarge : throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function transmits the requested drone data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Drone getDrone(int Id)
        {
            Drone getDrone = DataSource.drones.Find(Drone => Drone.Id == Id);
            return getDrone.Id != default ? getDrone : throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        /// <summary>
        /// This function transmits the requested customer data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer getCustomer(int Id)
        {
            Customer getCustomer = DataSource.customers.Find(Customer => Customer.Id == Id);
            return getCustomer.Id != default ? getCustomer : throw new IdNotExistExeptions("sorry, this customer is not found.");
        }
        /// <summary>
        /// This function transmits the requested parcel data according to an identification number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Parcel getParcel(int Id)
        {
            Parcel getParcel = DataSource.parcels.Find(Parcel => Parcel.Id == Id);
            return getParcel.Id != default ? getParcel : throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
        /// <summary>
        /// This function transmits data of all existing stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> DisplaysIistOfStations(Predicate<Station> p = null)
        {
            return DataSource.stations.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DisplaysTheListOfDrons(Predicate<Drone> p = null)
        {
            return DataSource.drones.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing customers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> DisplaysIistOfCustomers(Predicate<Customer> p = null)
        {
            return DataSource.customers.Where(d => p == null ? true : p(d)).ToList();
        }
        /// <summary>
        /// This function transmits data of all existing parcels.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplaysIistOfparcels(Predicate<Parcel> p = null)
        {
            return DataSource.parcels.Where(d => p == null ? true : p(d)).ToList();
        }
        public double[] PowerConsumptionRate()
        {
            double[] powerConsumptionRate = new double[] {
                DataSource.Config.available, DataSource.Config.easy,
                DataSource.Config.medium, DataSource.Config.Heavy,
                DataSource.Config.ChargingRate
            };
            return powerConsumptionRate;
        }
        public bool removeDrone(int id)
        {
            int find = DataSource.drones.FindIndex(Drone => Drone.Id == id);
            if (find != -1)
            {
                DataSource.drones.RemoveAt(find);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this Drone is not found.");
        }
        public bool removeStation(int id)
        {
            int find = DataSource.stations.FindIndex(Station => Station.Id == id);
            if (find != -1)
            {
                DataSource.stations.RemoveAt(find);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this Station is not found.");
        }
        public bool removeCustomer(int id)
        {
            int find = DataSource.customers.FindIndex(Customer => Customer.Id == id);
            if (find != -1)
            {
                DataSource.customers.RemoveAt(find);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this customer is not found.");
        }
        public bool removeParcel(int id)
        {
            int find = DataSource.parcels.FindIndex(Parcel => Parcel.Id == id);
            if (find != -1)
            {
                DataSource.parcels.RemoveAt(find);
                return true;
            }
            throw new IdNotExistExeptions("sorry, this Parcel is not found.");
        }
    }
}
