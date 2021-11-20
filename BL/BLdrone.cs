using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public partial class BL : IBL
    {

        public bool addDrone(Drone drone, int idStation = 0)
        {
            try
            {
                IDAL.DO.Station station = (IDAL.DO.Station)dal.getStation(idStation);//לבדוק לגבי ההמרה
                drone.Location.latitude = station.lattitude; drone.Location.longitude = station.longitude;

            IDAL.DO.Drone dalDrone = new()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight
            };
            bool test = dal.addDrone(dalDrone);
            if (!test)
                throw new NotImplementedException();

            bool cargeTest = dal.reductionCargeSlotsToStation(idStation);//Reduces loading slot.
            if (!cargeTest)
                throw new NotImplementedException();

            IDAL.DO.DroneCarge droneCarge = new() { DroneID = drone.Id, StationId = idStation };
            dal.addDroneCarge(droneCarge);//Builds a drone entity by charging.

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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public bool updateModelOfDrone(string newModel, int IdDrone)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public bool SendDroneForCharging(int IdDrone)
        {
            try
            {
                DroneToList drone = droneToLists.Find(i => i.Id == IdDrone);
                if (drone.DroneStatuses != DroneStatuses.available || drone.Id == 0)//לבדוק לגבי התקינות של הבדיקה עם null
                    throw new();//לטפל בחריגה המתאימה כאן ///////////////////////////////////////////////////////
                else
                {
                    List<IDAL.DO.Station> stationWithFreeSlots = dal.DisplaysIistOfStations(i => i.freeChargeSlots > 0).ToList();
                    IDAL.DO.Station closeStation = TheNearestStation(drone.Location, stationWithFreeSlots);
                    Location stationLocation = new() { latitude = closeStation.lattitude, longitude = closeStation.longitude };

                double KM = d.DistanceBetweenPlaces(drone.Location, stationLocation);//Looking for the nearest available station.

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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public bool ReleaseDroneFromCharging(int IdDrone, int time)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public bool AssignPackageToDrone(int IdDrone)
        {
            try
            {
                IDAL.DO.Parcel bestParcel;
                IDAL.DO.Drone drone = (IDAL.DO.Drone)dal.getDrone(IdDrone);

                DroneToList droneToList = droneToLists.Find(i => i.Id == IdDrone);
                int droneFind = droneToLists.FindIndex(i => i.Id == IdDrone);

                if (droneToList.DroneStatuses != DroneStatuses.available)//Checks if the drone is available.
                    throw new NotImplementedException();

                List<IDAL.DO.Parcel> parcels = dal.DisplaysIistOfparcels(i => i.Scheduled == DateTime.MinValue).ToList();
                if (parcels.Count == 0)
                    throw new NotImplementedException();

                bestParcel = AssignStep1(droneToList, parcels);//Receives from auxiliary functions the most suitable package for delivery.

                droneToLists[droneFind].DroneStatuses = DroneStatuses.busy;
                droneToLists[droneFind].parcelNumber = bestParcel.Id;

                bool test = dal.AssignPackageToDrone(bestParcel.Id, IdDrone);
                if (test)
                    return true;
                else
                    throw new NotImplementedException();
            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }

        }
        /// <summary>
        /// This function returns the list of relevant packages to the glider by examining the parameters of weight and distance, in order of urgency.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel AssignStep1(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {

            try
            {
                List<IDAL.DO.Parcel> emergency = new();
                List<IDAL.DO.Parcel> fast = new();
                List<IDAL.DO.Parcel> normal = new();

            foreach (IDAL.DO.Parcel item in parcels)
            {
                if (item.weight <= (IDAL.DO.WeightCategories)droneToList.MaxWeight)//Feasibility study by weight parameter.
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
                    //The distance of delivery does not include return to the charging station.
                    double batteryToTheDelivery;
                    batteryToTheDelivery = d.DistanceBetweenPlaces(droneToList.Location, sanderLocation) * available +
                        (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight);
                    if (droneToList.battery >= batteryToTheDelivery)// Feasibility study according to distance parameter
                    {
                        //Add the distance to the station only for the remaining relevant packages.
                        batteryToTheDelivery += (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);

                        if (droneToList.battery >= batteryToTheDelivery)//Feasibility study according to distance parameter
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
                if (emergency.Count == 0 && fast.Count == 0 && normal.Count == 0)//If there are no packages left that meet the necessary parameters.
                    throw new ThereIsNoSuitablePackage("There is no suitable package for the drone");
            }
            return AssignStep2(droneToList, (emergency.Count > 0) ? emergency : (fast.Count > 0) ? fast : normal);

            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function returns the list of most weighted packages.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel AssignStep2(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function returns the location of the package closest to the glider.
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel TheNearestParcelToAssign(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public bool PackageCollectionByDrone(int IdDrone)
        {
            try
            {
                IDAL.DO.Parcel parcelsOfDrone = dal.DisplaysIistOfparcels(i => i.DroneId == IdDrone && i.PickedUp == DateTime.MinValue).First();
                if (parcelsOfDrone.DroneId != IdDrone)
                    throw new NotImplementedException();

                IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(parcelsOfDrone.SenderId);
                Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                int index = droneToLists.FindIndex(i => i.Id == IdDrone);
                droneToLists[index].battery -= d.DistanceBetweenPlaces(droneToLists[index].Location, sanderLocation) * available;
                droneToLists[index].Location = sanderLocation;

            dal.removeParcel(parcelsOfDrone.Id);//Update by deleting an object in a previous configuration and readjusting after changes.
            parcelsOfDrone.PickedUp = DateTime.Now;
            dal.addParsel(parcelsOfDrone);

                return true;
            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public bool DeliveryPackageToCustomer(int IdDrone)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public Drone GetDrone(int droneId)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public DroneInParcel GetDroneInParcel(int droneId)
        {
            DroneToList droneToList = droneToLists.Find(i => i.Id == droneId);
            if (droneToList.Id == 0)
                throw new NotImplementedException();

            DroneInParcel droneInParcel = new()
            {
                Id = droneToList.Id,
                battery = droneToList.battery,
                Location = droneToList.Location
            };
            return droneInParcel;
        }
        public IEnumerable<DroneToList> DisplaysIistOfDrons(Predicate<DroneToList> p = null)
        {
            return droneToLists.Where(d => p == null ? true : p(d)).ToList();
        }
    }
}
