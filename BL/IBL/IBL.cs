using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {
        /// <summary>
        /// This function allows the user to add a station to the list.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        bool addStation(Station station);
        /// <summary>
        /// This function allows the user to add a drone to the list.
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="idStation"></param>
        /// <returns></returns>
        bool addDrone(Drone drone, int idStation = 0);
        /// <summary>
        /// This function allows the user to add a customer to the list.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool addCustomer(Customer customer);
        /// <summary>
        /// This function allows the user to add a parcel to the list.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        int addParsel(Parcel parcel);
        /// <summary>
        /// This function updates the drone model.
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        bool updateModelOfDrone(String newName, int IdDrone);
        /// <summary>
        /// This function updates the station data.
        /// </summary>
        /// <param name="Idstation"></param>
        /// <param name="newName"></param>
        /// <param name="ChargingSlots"></param>
        /// <returns></returns>
        bool updateStationData(int Idstation, string newName, int ChargingSlots);
        /// <summary>
        /// This function updates the customer data.
        /// </summary>
        /// <param name="IdCustomer"></param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
        /// <returns></returns>
        bool updateCustomerData(int IdCustomer, string newName, string newPhone);
        /// <summary>
        /// This function sends a drone for charging.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        bool SendDroneForCharging(int IdDrone);
        /// <summary>
        /// This function releases a drone from a charger.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        bool ReleaseDroneFromCharging(int IdDrone, int time);
        /// <summary>
        /// This function assigns a package to the drone.(By three functions: AssignStep1, AssignStep2, and TheNearestParcelToAssign).
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        bool AssignPackageToDrone(int IdDrone);
        /// <summary>
        /// This function performs an update on packet collection by drone.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        bool PackageCollectionByDrone(int IdDrone);
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        bool DeliveryPackageToCustomer(int IdDrone);
        /// <summary>
        /// This function finds the next closest package to the drone.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public DO.Parcel TheNearestParcelToAssign(DroneToList droneToList, List<DO.Parcel> parcels);
        /// <summary>
        /// This function returns the Customer In Parcel object
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerInParcel GetCustomerInParcel(int customerId);
        /// <summary>
        /// This function returns the Customer object
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerId);
        /// <summary>
        /// This function returns the Drone object
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId);
        /// <summary>
        /// This function returns the Drone In Parcel object
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public DroneInParcel GetDroneInParcel(int droneId);
        /// <summary>
        /// This function returns the station object
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId);

        /// <summary>
        /// This function returns the Parcel object
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId);
        /// <summary>
        /// This function returns the Package In Transfer object
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public PackageInTransfer GetPackageInTransfer(Location location,  int parcelId);
        /// <summary>
        /// This function returns the logical entity ParcelToList.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ParcelInCustomer GetParcelInCustomer(int parcelId, int customerId);
        /// <summary>
        /// This function returns the logical entity ParcelToList.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public ParcelToList GetParcelToList(int parcelId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<DroneToList> DisplaysIistOfDrons(Predicate<DroneToList> p = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<StationToList> DisplaysIistOfStations(Predicate<StationToList> p = null);
        /// <summary>
        /// this function get the id of close station to the drone.
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        public int GetTheIdOfCloseStation(int idDrone);
        /// <summary>
        ///  this function get the id of close parcels to the drone.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<ParcelToList> DisplaysIistOfparcels(Predicate<ParcelToList> p = null);
        /// <summary>
        ///  this function get the id of close parcels to the drone.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<CustomerToList> DisplaysIistOfCustomers(Predicate<CustomerToList> p = null);
        /// <summary>
        /// This function deletes a package if it has not yet been associated with a drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public bool remuveParcel(int parcelId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="updateDrone"></param>
        /// <param name="cancellationThreading"></param>
        public void newSimulator(int droneId, Action updateDrone, Func<bool> cancellationThreading);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        public Location TheLocationForTheNearestStation(Location customerLocation, List<DO.Station> stations);

    }
}
