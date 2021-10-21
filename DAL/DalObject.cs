using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalObject
    {
        public DalObject() => DataSource.Initialize();
        public void addStation(Station station)
        {

            //int Id = DataSource.Config.StationIdRun++;
            //Console.WriteLine("enter name to the station");
            //string name=Console.ReadLine();
            //Console.WriteLine("enter longitudes of the station ");
            //double longitude = Console.Read();
            //Console.WriteLine("enter Latitudes of the station ");
            //double lattitude=Console.Read();
            //Console.WriteLine("enter number of CargeSlots");
            //int ChargeSlots = Console.Read();

            int i = DataSource.Config.StationIndex++;
            DataSource.stations[i] = station;

        }

        public void addDrone(Drone drone)
        {
            //int Id = DataSource.Config.DroneIdRun++;
            //Console.WriteLine("enter model of the drone");
            //string model = Console.ReadLine();
            //Console.WriteLine("entermaximum weight for the drone: 1 to ");
            //double longitude = Console.Read();
            //Console.WriteLine("enter Latitudes of the station ");
            //double lattitude = Console.Read();
            //Console.WriteLine("enter number of CargeSlots");
            //int ChargeSlots = Console.Read();

            int i = DataSource.Config.DroneIndex++;
            DataSource.drones[i] = drone;
        }
        public void addCustomer(Customer customer)
        {
            int i = DataSource.Config.CustomerIndex++;
            DataSource.Customers[i] = customer;
        }
        public void addParsel(Parcel parcel)
        {
            int i = DataSource.Config.ParcelIndex++;
            DataSource.Parcels[i] = parcel;
        }
        public void AssignPackageToDrone(Parcel parcel, Drone drone)
        {
            parcel.DroneId = drone.Id;
            parcel.Scheduled = DateTime.Now;
        }
        public void PackageCollectionByDrone(Parcel parcel, Drone drone)
        {
            drone.Status = DroneStatuses.busy;//לבדוק האם נכון
            parcel.PickedUp = DateTime.Now;
        }
        public void DeliveryPackageToCustomer(Parcel parcel, Drone drone)
        {
            parcel.Delivered = DateTime.Now;
            drone.Status = DroneStatuses.available;//לבדוק האם נכון
        }
        public void SendingDroneForCharging(Drone drone, Station station)//יש לזכור במיין להוסיף זימון להצגת תחנות פנויות
        {
            int i = DataSource.Config.DroneCargeIndex++;
            drone.Status = DroneStatuses.maintenance;
            DataSource.DroneCarges[i].DroneID = drone.Id;
            DataSource.DroneCarges[i].StationId = station.Id;//לא מובן לאן זה נשלח ואיך זה נשמר
            station.ChargeSlots--;
        }
        public void ReleaseDroneFromCharging(Drone drone, Station station)
        {
            
        }
        public void 
            {

             }
    }
}
