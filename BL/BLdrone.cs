using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public partial class BL : IBL
    {
        /// <summary>
        /// This function allows the user to add a drone to the list.
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="idStation"></param>
        /// <returns></returns>
        public bool addDrone(Drone drone, int idStation = 0)//לזכור לעשות try catch
        {
            IDAL.DO.Station station = (IDAL.DO.Station)dal.getStation(idStation);//לבדוק לגבי ההמרה
            drone.Location.latitude = station.lattitude; drone.Location.longitude = station.longitude;

            IDAL.DO.Drone dalDrone = new()//לזכור לקחת פונ' מיהודה שור
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight
            };
            bool test = dal.addDrone(dalDrone);
            if (!test)
                throw new NotImplementedException();

            bool cargeTest = dal.reductionCargeSlotsToStation(idStation);//לבדוק מה קורה עם השגיאה שתיזרק מהפונ' הזו.
            if (!cargeTest)
                throw new NotImplementedException();

            IDAL.DO.DroneCarge droneCarge = new() { DroneID = drone.Id, StationId = idStation };
            dal.addDroneCarge(droneCarge);

            droneToLists.Add(new DroneToList()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Location = drone.Location,
                battery = (random.NextDouble() * (20.0)) + 20.0,
                DroneStatuses = DroneStatuses.maintenance,
            });
            return true;
        }
        /// <summary>
        /// This function updates the drone model.
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        public bool updateModelOfDrone(string newModel, int IdDrone)
        {
            IDAL.DO.Drone tempDrone = (IDAL.DO.Drone)dal.getDrone(IdDrone);//לבדוק לגבי ההמרה
            dal.removeDrone(IdDrone);
            tempDrone.Model = newModel;
            bool test = dal.addDrone(tempDrone);//הנחתי שהבוליאניות היא רק לגבי ההוספה חזרה?
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }
        /// <summary>
        /// This function sends a drone for charging.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        public bool SendDroneForCharging(int IdDrone)
        {
            DroneToList drone = droneToLists.Find(i => i.Id == IdDrone);
            if (drone.DroneStatuses != DroneStatuses.available || drone.Id == 0)//לבדוק לגבי התקינות של הבדיקה עם null
                throw new();//לטפל בחריגה המתאימה כאן ///////////////////////////////////////////////////////
            else
            {
                List<IDAL.DO.Station> stationWithFreeSlots = dal.DisplaysIistOfStations(i => i.freeChargeSlots > 0).ToList();
                IDAL.DO.Station closeStation = TheNearestStation(drone.Location, stationWithFreeSlots);
                Location stationLocation = new() { latitude = closeStation.lattitude, longitude = closeStation.longitude };

                double KM = d.DistanceBetweenPlaces(drone.Location, stationLocation);

                if (drone.battery < (KM * available))
                    throw new();//לטפל בחריגה המתאימה כאן ///////////////////////////////////////////////////////
                else
                {
                    int droneIndex = droneToLists.FindIndex(i => i.Id == IdDrone);
                    drone.battery -= (KM * available);
                    drone.Location = stationLocation;
                    drone.DroneStatuses = DroneStatuses.maintenance;
                    droneToLists[droneIndex] = drone;

                    bool cargeTest = dal.reductionCargeSlotsToStation(closeStation.Id);//לבדוק מה קורה עם השגיאה שתיזרק מהפונ' הזו.
                    if (!cargeTest)
                        throw new NotImplementedException();
                    IDAL.DO.DroneCarge droneCarge = new() { DroneID = drone.Id, StationId = closeStation.Id };
                    dal.addDroneCarge(droneCarge);
                }
            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// This function releases a drone from a charger.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool ReleaseDroneFromCharging(int IdDrone, int time)
        {

            DroneToList drone = droneToLists.Find(i => i.Id == IdDrone);
            if (drone.DroneStatuses != DroneStatuses.maintenance || drone == null)//לבדוק לגבי התקינות של הבדיקה עם null
                throw new();//לטפל בחריגה המתאימה כאן ///////////////////////////////////////////////////////
            else
            {
                int droneIndex = droneToLists.FindIndex(i => i.Id == IdDrone);
                drone.battery = time * ChargingRate;
                drone.DroneStatuses = DroneStatuses.available;
                droneToLists[droneIndex] = drone;

                IDAL.DO.DroneCarge droneCarge = (IDAL.DO.DroneCarge)dal.getDroneCargeByDroneId(IdDrone);//לבדוק לגבי ההמרה
                bool addingTest = dal.addingCargeSlotsToStation(droneCarge.StationId);
                bool removeTest = dal.ReleaseDroneCarge(IdDrone);
            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// This function assigns a package to the drone.(By three functions: AssignStep1, AssignStep2, and TheNearestParcelToAssign).
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        public bool AssignPackageToDrone(int IdDrone)
        {
            IDAL.DO.Parcel bestParcel;

            //לבדוק האם צריך את המשתנה הזה
            IDAL.DO.Drone drone = (IDAL.DO.Drone)dal.getDrone(IdDrone);//לבדוק לגבי ההמרה

            DroneToList droneToList = droneToLists.Find(i => i.Id == IdDrone);
            int droneFind = droneToLists.FindIndex(i => i.Id == IdDrone);

            if (droneToList.DroneStatuses != DroneStatuses.available)
                throw new NotImplementedException();

            List<IDAL.DO.Parcel> parcels = dal.DisplaysIistOfparcels(i => i.Scheduled == DateTime.MinValue).ToList();
            if (parcels.Count == 0)
                throw new NotImplementedException();

            bestParcel = AssignStep1(droneToList, parcels);
            droneToLists[droneFind].DroneStatuses = DroneStatuses.busy;
            droneToLists[droneFind].parcelNumber = bestParcel.Id;

            bool test = dal.AssignPackageToDrone(bestParcel.Id, IdDrone);
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }
        /// <summary>
        /// This function returns the list of relevant packages to the glider in terms of shekel and distance, in order of their urgency.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel AssignStep1(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {

            List<IDAL.DO.Parcel> emergency = new();
            List<IDAL.DO.Parcel> fast = new();
            List<IDAL.DO.Parcel> normal = new();

            foreach (IDAL.DO.Parcel item in parcels)
            {
                if (item.weight <= (IDAL.DO.WeightCategories)droneToList.MaxWeight)
                {

                    IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(item.SenderId);
                    Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                    IDAL.DO.Customer target = (IDAL.DO.Customer)dal.getCustomer(item.TargetId);
                    Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                    double weight = easy;
                    switch (item.weight)
                    {
                        case IDAL.DO.WeightCategories.easy:
                            weight = easy;
                            break;
                        case IDAL.DO.WeightCategories.medium:
                            weight = medium;
                            break;
                        case IDAL.DO.WeightCategories.heavy:
                            weight = Heavy;
                            break;
                        default:
                            break;
                    }
                    double batteryToTheDelivery;
                    batteryToTheDelivery = d.DistanceBetweenPlaces(droneToList.Location, sanderLocation) * available +
                        (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight);
                    if (droneToList.battery >= batteryToTheDelivery)
                    {
                        batteryToTheDelivery += (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);

                        if (droneToList.battery >= batteryToTheDelivery)
                        {
                            switch (item.priority)
                            {
                                case IDAL.DO.Priorities.normal:
                                    normal.Add(item);
                                    break;
                                case IDAL.DO.Priorities.fast:
                                    fast.Add(item);
                                    break;
                                case IDAL.DO.Priorities.emergency:
                                    emergency.Add(item);
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                }
                if (emergency.Count == 0 && fast.Count == 0 && normal.Count == 0)
                    throw new NotImplementedException();
            }
            return AssignStep2(droneToList, (emergency.Count > 0) ? emergency : (fast.Count > 0) ? fast : normal);

        }
        /// <summary>
        /// This function returns the list of most weighted packages.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel AssignStep2(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {
            List<IDAL.DO.Parcel> easy = new();
            List<IDAL.DO.Parcel> medium = new();
            List<IDAL.DO.Parcel> heavy = new();
            foreach (IDAL.DO.Parcel item in parcels)
            {
                switch (item.weight)
                {
                    case IDAL.DO.WeightCategories.easy:
                        easy.Add(item);
                        break;
                    case IDAL.DO.WeightCategories.medium:
                        medium.Add(item);
                        break;
                    case IDAL.DO.WeightCategories.heavy:
                        heavy.Add(item);
                        break;
                    default:
                        break;
                }
            }
            return TheNearestParcelToAssign(droneToList, (heavy.Count > 0) ? heavy : (medium.Count > 0) ? medium : easy);
        }
        /// <summary>
        /// This function returns the location of the package closest to the glider.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel TheNearestParcelToAssign(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {
            IDAL.DO.Parcel closeParcel = new();

            IDAL.DO.Parcel closeParsel = parcels[0];
            double distance = double.MaxValue;

            foreach (IDAL.DO.Parcel item in parcels)
            {
                IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(item.SenderId);
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
        /// <summary>
        /// This function performs an update on packet collection by drone.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        public bool PackageCollectionByDrone(int IdDrone)
        {
            IDAL.DO.Parcel parcelsOfDrone = dal.DisplaysIistOfparcels(i => i.DroneId == IdDrone && i.PickedUp == DateTime.MinValue).First();
            if (parcelsOfDrone.DroneId != IdDrone)
                throw new NotImplementedException();

            IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(parcelsOfDrone.SenderId);
            Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

            int index = droneToLists.FindIndex(i => i.Id == IdDrone);
            droneToLists[index].battery -= d.DistanceBetweenPlaces(droneToLists[index].Location, sanderLocation) * available;
            droneToLists[index].Location = sanderLocation;

            dal.removeParcel(parcelsOfDrone.Id);
            parcelsOfDrone.PickedUp = DateTime.Now;
            dal.addParsel(parcelsOfDrone);

            return true;
        }
        /// <summary>
        /// This function performs an update on delivering a package to the customer.
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <returns></returns>
        public bool DeliveryPackageToCustomer(int IdDrone)
        {
            IDAL.DO.Parcel parcelsOfDrone = dal.DisplaysIistOfparcels(i => i.DroneId == IdDrone && i.PickedUp != DateTime.MinValue && i.Delivered == DateTime.MinValue).First();
            if (parcelsOfDrone.DroneId != IdDrone)
                throw new NotImplementedException();

            IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(parcelsOfDrone.SenderId);
            Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

            IDAL.DO.Customer target = (IDAL.DO.Customer)dal.getCustomer(parcelsOfDrone.TargetId);
            Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

            int index = droneToLists.FindIndex(i => i.Id == IdDrone);

            double weight = easy;
            switch (parcelsOfDrone.weight)
            {
                case IDAL.DO.WeightCategories.easy:
                    weight = easy;
                    break;
                case IDAL.DO.WeightCategories.medium:
                    weight = medium;
                    break;
                case IDAL.DO.WeightCategories.heavy:
                    weight = Heavy;
                    break;
                default:
                    break;
            }

            droneToLists[index].battery -= d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight;
            droneToLists[index].Location = targetLocation;
            droneToLists[index].DroneStatuses = DroneStatuses.available;
            droneToLists[index].parcelNumber = 0;


            dal.removeParcel(parcelsOfDrone.Id);
            parcelsOfDrone.Delivered = DateTime.Now;
            dal.addParsel(parcelsOfDrone);

            return true;
        }
        public Drone GetDrone(int droneId)
        {
            DroneToList droneToList = droneToLists.Find(i => i.Id == droneId);
            if (droneToList.Id == 0)
                throw new NotImplementedException();
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
                drone.packageInTransfer = GetPackageInTransfer(droneToList.parcelNumber);

            return drone;
        }
    }
}
