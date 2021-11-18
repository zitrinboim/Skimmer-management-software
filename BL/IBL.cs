using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL.BO
{
    public interface IBL
    {
        bool addStation(Station station);
        bool addDrone(Drone drone, int idStation = 0);//לעדכן את המיקום של הרחפן להתחנה אם קיימת וכל הנגזר מזה
        bool addCustomer(Customer customer);
        int addParsel(IDAL.DO.Parcel parcel);
        bool updateModelOfDrone(String newName, int IdDrone);
        bool updateStationData(int Idstation, string newName, int ChargingSlots);
        bool updateCustomerData(int IdCustomer, string newName,string newPhone);
        bool SendDroneForCharging(int IdDrone);
        bool ReleaseDroneFromCharging(int IdDrone, int time);
        bool AssignPackageToDrone(int IdDrone);
        bool PackageCollectionByDrone(int IdDrone);
        bool DeliveryPackageToCustomer(int IdDrone);
        IDAL.DO.Parcel AssignStep1(DroneToList droneToList, List<IDAL.DO.Parcel> parcels);
        IDAL.DO.Parcel AssignStep2(DroneToList droneToList, List<IDAL.DO.Parcel> parcels);
        public IDAL.DO.Parcel TheNearestParcelToAssign(DroneToList droneToList, List<IDAL.DO.Parcel> parcels);

    }
}
