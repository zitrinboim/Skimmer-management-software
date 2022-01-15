using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="updateDrone"></param>
        /// <param name="cancellationThreading"></param>
        public void droneSimolat(int droneId, Action updateDrone, Func<bool> cancellationThreading)
        {
            new DroneSimulator(this, droneId, updateDrone, cancellationThreading);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="idStation"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool addDrone(Drone drone, int idStation = 0)
        {
            try
            {
                DO.Station station = dal.getStation(idStation);
                drone.Location = new() { latitude = station.lattitude, longitude = station.longitude };

                DO.Drone dalDrone = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (DO.WeightCategories)drone.MaxWeight
                };

                _ = dal.addDrone(dalDrone);

                _ = dal.reductionCargeSlotsToStation(idStation);//Reduces loading slot.

                DO.DroneCarge droneCarge = new() { DroneID = drone.Id, StationId = idStation };
                dal.addDroneCarge(droneCarge);//Builds a drone entity by charging.

                droneToLists.Add(new DroneToList()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Location = drone.Location,
                    battery = ((random.NextDouble() * (20.0)) + 20.0),
                    DroneStatuses = DroneStatuses.maintenance,
                });
                return true;
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
        /// 
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool updateModelOfDrone(string newModel, int IdDrone)
        {
            try
            {
                DO.Drone tempDrone = dal.getDrone(IdDrone);
                dal.removeDrone(IdDrone);
                tempDrone.Model = newModel;
                _ = dal.addDrone(tempDrone);
                return true;
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
        /// 
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SendDroneForCharging(int IdDrone)
        {
            try
            {
                DroneToList drone = droneToLists.Find(i => i.Id == IdDrone);
                if (drone.DroneStatuses != DroneStatuses.available || drone.Id == 0)
                    throw new ChargingExeptions("לא יכול להשלח לטעינה");
                else
                {
                    List<DO.Station> stationWithFreeSlots = dal.DisplaysIistOfStations(i => i.freeChargeSlots > 0).ToList();
                    DO.Station closeStation = TheNearestStation(drone.Location, stationWithFreeSlots);
                    Location stationLocation = new() { latitude = closeStation.lattitude, longitude = closeStation.longitude };

                    double KM = d.DistanceBetweenPlaces(drone.Location, stationLocation);//Looking for the nearest available station.

                    if (drone.battery < (KM * available))
                        throw new ChargingExeptions("אין מספיק בטרייה להגעה לתחנה"); 
                    else
                    {
                        int droneIndex = droneToLists.FindIndex(i => i.Id == IdDrone);
                        drone.battery -= (KM * available);
                        drone.Location = stationLocation;
                        drone.DroneStatuses = DroneStatuses.maintenance;
                        droneToLists[droneIndex] = drone;

                        _ = dal.reductionCargeSlotsToStation(closeStation.Id);
                        dal.addDroneCarge(new() { DroneID = drone.Id, StationId = closeStation.Id });
                        return true;
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
        /// 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetTheIdOfCloseStation(int idDrone)
        {
            Drone drone = GetDrone(idDrone);
            List<DO.Station> stationWithFreeSlots = dal.DisplaysIistOfStations().ToList();
            DO.Station closeStation = TheNearestStation(drone.Location, stationWithFreeSlots);
            return closeStation.Id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ReleaseDroneFromCharging(int IdDrone, int time)
        {
            try
            {
                DroneToList drone = droneToLists.Find(i => i.Id == IdDrone);
                if (drone.DroneStatuses != DroneStatuses.maintenance || drone.Id == 0)
                    throw new ChargingExeptions("אין רחפן כזה בטעינה");
                else
                {
                    int droneIndex = droneToLists.FindIndex(i => i.Id == IdDrone);
                    drone.battery += (time * ChargingRate);
                    if (drone.battery > 100)
                        drone.battery = 100;
                    drone.DroneStatuses = DroneStatuses.available;
                    droneToLists[droneIndex] = drone;

                    DO.DroneCarge droneCarge = dal.getDroneCargeByDroneId(IdDrone);
                    bool addingTest = dal.addingCargeSlotsToStation(droneCarge.StationId);
                    bool removeTest = dal.ReleaseDroneCarge(IdDrone);
                    return true;
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
        /// 
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool AssignPackageToDrone(int IdDrone)
        {
            try
            {
                DO.Parcel bestParcel = new();
                DO.Drone drone = dal.getDrone(IdDrone);

                DroneToList droneToList = droneToLists.Find(i => i.Id == IdDrone);
                int droneFind = droneToLists.FindIndex(i => i.Id == IdDrone);

                if (droneToList.DroneStatuses != DroneStatuses.available)//Checks if the drone is available.
                    throw new PackageAssociationExeptions("רחפן לא פנוי למשלוח");

                bestParcel = (from Parcel in dal.DisplaysIistOfparcels().ToList()
                              where Parcel.Scheduled == DateTime.MinValue
                              orderby Parcel.priority descending
                              orderby Parcel.weight descending
                              where batteryCalculation(droneToList, Parcel)
                              select Parcel).FirstOrDefault();

                if (bestParcel.Id != 0)
                {
                    droneToLists[droneFind].DroneStatuses = DroneStatuses.busy;
                    droneToLists[droneFind].parcelNumber = bestParcel.Id;

                    _ = dal.AssignPackageToDrone(bestParcel.Id, IdDrone);
                    return true;
                }
                else
                    throw new PackageAssociationExeptions("אין חבילה מתאימה עבור רחפן זה");
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
        /// This function calculates whether the battery is sufficient for the shipment of this package.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private bool batteryCalculation(DroneToList droneToList, DO.Parcel parcel)
        {
            DO.Customer sander = dal.getCustomer(parcel.SenderId);
            Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

            DO.Customer target = dal.getCustomer(parcel.TargetId);
            Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

            double weight = easy;
            switch (parcel.weight)
            {
                case DO.WeightCategories.easy:
                    weight = easy;
                    break;
                case DO.WeightCategories.medium:
                    weight = medium;
                    break;
                case DO.WeightCategories.heavy:
                    weight = Heavy;
                    break;
                default:
                    break;
            }

            if ((d.DistanceBetweenPlaces(droneToList.Location, sanderLocation) * available) +
                (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight) +
                (d.DistanceBetweenPlaces(targetLocation,
                TheLocationForTheNearestStation(targetLocation, stations)) * available) <= droneToList.battery)
                return true;
            else
                return false;

        }
        /// <summary>
        /// This function returns the location of the package closest to the glider.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Parcel TheNearestParcelToAssign(DroneToList droneToList, List<DO.Parcel> parcels)
        {
            try
            {
                DO.Parcel closeParcel = new();

                DO.Parcel closeParsel = parcels[0];
                double distance = double.MaxValue;

                foreach (DO.Parcel item in parcels)
                {
                    DO.Customer sander = dal.getCustomer(item.SenderId);
                    Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                    double distance1 = d.DistanceBetweenPlaces(droneToList.Location, sanderLocation);
                    if (distance1 < distance)
                    {
                        distance = distance1;
                        closeParcel = item;
                    }
                }
                return closeParcel;
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
        /// 
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool PackageCollectionByDrone(int IdDrone)
        {
            try
            {
                DO.Parcel parcelsOfDrone = dal.DisplaysIistOfparcels(i => i.DroneId == IdDrone && i.PickedUp == DateTime.MinValue).First();

                DO.Customer sander = dal.getCustomer(parcelsOfDrone.SenderId);
                Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                int index = droneToLists.FindIndex(i => i.Id == IdDrone);
                droneToLists[index].battery -= d.DistanceBetweenPlaces(droneToLists[index].Location, sanderLocation) * available;
                droneToLists[index].Location = sanderLocation;
                dal.PackageCollectionByDrone(parcelsOfDrone.Id);
                return true;
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
        /// 
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeliveryPackageToCustomer(int IdDrone)
        {
            try
            {
                DO.Parcel parcelsOfDrone = dal.DisplaysIistOfparcels(i => i.DroneId == IdDrone && i.PickedUp != DateTime.MinValue && i.Delivered == DateTime.MinValue).First();
               
                DO.Customer sander = dal.getCustomer(parcelsOfDrone.SenderId);
                Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                DO.Customer target = dal.getCustomer(parcelsOfDrone.TargetId);
                Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                int index = droneToLists.FindIndex(i => i.Id == IdDrone);

                double weight = easy;
                switch (parcelsOfDrone.weight)
                {
                    case DO.WeightCategories.easy:
                        weight = easy;
                        break;
                    case DO.WeightCategories.medium:
                        weight = medium;
                        break;
                    case DO.WeightCategories.heavy:
                        weight = Heavy;
                        break;
                    default:
                        break;
                }

                droneToLists[index].battery -= d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight;
                droneToLists[index].Location = targetLocation;
                droneToLists[index].DroneStatuses = DroneStatuses.available;
                droneToLists[index].parcelNumber = 0;
                dal.DeliveryPackageToCustomer(parcelsOfDrone.Id);

                return true;
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
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            try
            {
                DroneToList droneToList = new();
                droneToList = droneToLists.Find(i => i.Id == droneId);
                if (droneToList == default)
                    throw new IdNotExistExeptions("error");
                Drone drone = new()
                {
                    Id = droneToList.Id,
                    battery = droneToList.battery,
                    DroneStatuses = droneToList.DroneStatuses,
                    Location = droneToList.Location,
                    MaxWeight = droneToList.MaxWeight,
                    Model = droneToList.Model
                };
                if (droneToList.parcelNumber != 0)
                    drone.packageInTransfer = GetPackageInTransfer(droneToList.Location, droneToList.parcelNumber);

                return drone;
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
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneInParcel GetDroneInParcel(int droneId)
        {
            DroneToList droneToList = droneToLists.Find(i => i.Id == droneId);
            if (droneToList.Id == 0)
                throw new IdNotExistExeptions("אין רחפן עם מזהה זה");
            DroneInParcel droneInParcel = new()
            {
                Id = droneToList.Id,
                battery = droneToList.battery,
                Location = droneToList.Location
            };
            return droneInParcel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> DisplaysIistOfDrons(Predicate<DroneToList> p = null)
        {
            return droneToLists.Where(d => p == null ? true : p(d)).ToList();
        }
    }
}
