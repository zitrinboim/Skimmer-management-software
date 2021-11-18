using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL.BO
{
    public partial class BL : IBL
    {
        List<DroneToList> droneToLists;
        IDAL.IDal dal;
        DistanceAlgorithm d;
        Random random;
        List<IDAL.DO.Station> stations;
        List<IDAL.DO.Customer> customers;
        List<IDAL.DO.Drone> drones;
        List<IDAL.DO.Parcel> PackagesInDelivery;

        internal static double available;
        internal static double easy;
        internal static double medium;
        internal static double Heavy;
        internal static double ChargingRate;
        public BL()
        {
            droneToLists = new();
            d = new();
            dal = new Dal.DalObject();
            double battryOfDelivery;
            random = new Random(DateTime.Now.Millisecond);

            double[] power = dal.PowerConsumptionRate();//לאתחל את הערכים של הצריכה 
            available = power[0];
            easy = power[1];
            medium = power[2];
            Heavy = power[3];
            ChargingRate = power[4];//אולי להוציא לפונ' נפרדת.

            stations = dal.DisplaysIistOfStations().ToList();
            customers = dal.DisplaysIistOfCustomers().ToList();
            drones = dal.DisplaysTheListOfDrons().ToList();
            PackagesInDelivery = dal.DisplaysIistOfparcels(i => i.DroneId != 0).ToList();

            for (int i = 0; i < drones.Count; i++)// לקחת פונ' מיהודה שור
            {
                droneToLists[i].Id = drones[i].Id;
                droneToLists[i].Model = drones[i].Model;
                droneToLists[i].MaxWeight = (WeightCategories)drones[i].MaxWeight;
            }

            foreach (DroneToList drone in droneToLists)
            {
                int find = PackagesInDelivery.FindIndex(i => i.DroneId == drone.Id && i.Delivered == DateTime.MinValue);
                if (find != -1)
                {
                    drone.DroneStatuses = DroneStatuses.busy;

                    IDAL.DO.Customer sander = customers.Find(customer => customer.Id == PackagesInDelivery[find].SenderId);
                    IDAL.DO.Customer target = customers.Find(customer => customer.Id == PackagesInDelivery[find].TargetId);
                    Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };
                    Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                    if (PackagesInDelivery[find].PickedUp == DateTime.MinValue)
                    {
                        drone.Location = TheLocationForTheNearestStation(sanderLocation, stations);
                        battryOfDelivery = (d.DistanceBetweenPlaces(drone.Location, sanderLocation) * available)
                        + (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])
                        + (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);
                        drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                    }
                    else
                    {
                        drone.Location = sanderLocation;
                        battryOfDelivery = (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * power[(int)PackagesInDelivery[find].weight])//להסביר
                        + (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);
                        drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                    }
                }
                else
                {
                    int index;

                    drone.DroneStatuses = (DroneStatuses)random.Next(1, 3);
                    if (drone.DroneStatuses == (DroneStatuses)1)
                    {
                        List<IDAL.DO.Parcel> droneParcels = PackagesInDelivery.FindAll(i => i.DroneId == drone.Id && i.Delivered != DateTime.MinValue);
                        index = random.Next(0, droneParcels.Count);
                        IDAL.DO.Customer target = customers.Find(customer => customer.Id == droneParcels[index].TargetId);
                        drone.Location.latitude = target.lattitude;
                        drone.Location.longitude = target.longitude;

                        battryOfDelivery = d.DistanceBetweenPlaces(drone.Location, TheLocationForTheNearestStation(drone.Location, stations)) * available;
                        drone.battery = (random.NextDouble() * (100.0 - battryOfDelivery)) + battryOfDelivery;
                    }
                    else
                    {
                        index = random.Next(0, stations.Count);
                        drone.Location.latitude = stations[index].lattitude;
                        drone.Location.longitude = stations[index].longitude;
                        drone.battery = random.NextDouble() * 20.0;
                    }
                }
            }
        }
        private Location TheLocationForTheNearestStation(Location customerLocation, List<IDAL.DO.Station> stations)
        {
            IDAL.DO.Station closeStation = stations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < stations.Count; i++)
            {
                Location stationLocation = new() { latitude = stations[i].lattitude, longitude = stations[i].longitude };
                double distance1 = d.DistanceBetweenPlaces(stationLocation, customerLocation);
                if (distance1 < distance)
                {
                    distance = distance1;
                    closeStation = stations[i];
                }
            }
            Location location = new() { longitude = closeStation.longitude, latitude = closeStation.lattitude };
            return location;
        }
        private IDAL.DO.Station TheNearestStation(Location customerLocation, List<IDAL.DO.Station> stations)
        {
            IDAL.DO.Station closeStation = stations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < stations.Count; i++)
            {
                Location stationLocation = new() { latitude = stations[i].lattitude, longitude = stations[i].longitude };
                double distance1 = d.DistanceBetweenPlaces(stationLocation, customerLocation);
                if (distance1 < distance)
                {
                    distance = distance1;
                    closeStation = stations[i];
                }
            }
            Location location = new() { longitude = closeStation.longitude, latitude = closeStation.lattitude };
            return closeStation;
        }

        public bool addStation(Station station)//לזכור לעשות try catch 
        {
            IDAL.DO.Station dalStation = new()
            {
                Id = station.Id,
                name = station.name,
                freeChargeSlots = station.freeChargeSlots,
                lattitude = station.location.latitude,
                longitude = station.location.longitude
            };
            bool test = dal.addStation(dalStation);
            if (test)
                return true;
            else
                throw new NotImplementedException();//לבדוק  איזה חריגה לשים כאן.
        }

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

        public bool addCustomer(Customer customer)
        {
            IDAL.DO.Customer dalCustomer = new()
            {
                Id = customer.Id,
                name = customer.name,
                phone = customer.phone,
                lattitude = customer.location.latitude,
                longitude = customer.location.longitude
            };
            bool test = dal.addCustomer(dalCustomer);
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }

        public int addParsel(int sanderId, int targetId, WeightCategories weightCategories, Priorities priorities)
        {
            IDAL.DO.Parcel dalParcel = new()
            {
                Id = 0,
                SenderId = sanderId,
                TargetId = targetId,
                weight = (IDAL.DO.WeightCategories)weightCategories,
                priority = (IDAL.DO.Priorities)priorities,
                DroneId = 0,
                Requested = DateTime.Now,
                Delivered = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Scheduled = DateTime.MinValue
            };
            int addParcel = dal.addParsel(dalParcel);
            if (addParcel < 0)
                throw new NotImplementedException();
            return addParcel;

        }

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

        public bool updateStationData(int Idstation, string newName, int ChargingSlots)
        {
            IDAL.DO.Station tempStation = (IDAL.DO.Station)dal.getStation(Idstation);//לבדוק לגבי ההמרה
            dal.removeStation(Idstation);
            if (newName != "X" && newName != "x")
                tempStation.name = newName;
            if (ChargingSlots != -1)
                tempStation.freeChargeSlots = ChargingSlots;
            bool test = dal.addStation(tempStation);//הנחתי שהבוליאניות היא רק לגבי ההוספה חזרה
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }

        public bool updateCustomerData(int IdCustomer, string newName, string newPhone)
        {
            IDAL.DO.Customer tempCustomer = (IDAL.DO.Customer)dal.getCustomer(IdCustomer);//לבדוק לגבי ההמרה
            dal.removeCustomer(IdCustomer);
            if (newName != "X" && newName != "x")
                tempCustomer.name = newName;
            if (newPhone != "X" && newPhone != "x")
                tempCustomer.phone = newPhone;
            bool test = dal.addCustomer(tempCustomer);//הנחתי שהבוליאניות היא רק לגבי ההוספה חזרה
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }

        public bool SendDroneForCharging(int IdDrone)
        {
            DroneToList drone = droneToLists.Find(i => i.Id == IdDrone);
            if (drone.DroneStatuses != DroneStatuses.available || drone == null)//לבדוק לגבי התקינות של הבדיקה עם null
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

        public bool AssignPackageToDrone(int IdDrone)
        {
            IDAL.DO.Parcel bestParcel;
            List<IDAL.DO.Parcel> emergency = new();
            List<IDAL.DO.Parcel> fast = new();
            List<IDAL.DO.Parcel> normal = new();

            IDAL.DO.Drone drone = (IDAL.DO.Drone)dal.getDrone(IdDrone);//לבדוק לגבי ההמרה

            DroneToList droneToList = droneToLists.Find(i => i.Id == IdDrone);
            int droneFind = droneToLists.FindIndex(i => i.Id == IdDrone);

            if (droneToList.DroneStatuses != DroneStatuses.available)
                throw new NotImplementedException();

            List<IDAL.DO.Parcel> parcels = dal.DisplaysIistOfparcels(i => i.Scheduled == DateTime.MinValue).ToList();
            if (parcels.Count == 0)
                throw new NotImplementedException();


            //foreach (IDAL.DO.Parcel item in parcels)//לבדוק אופציה של מיון בתוך הרשימות ע"י מרחק והפונציה של המרחק
            //{
            //    if (item.priority == IDAL.DO.Priorities.emergency)//לבדוק אם יש איזה סוויטש קייס יפה לזה.
            //        emergency.Add(item);
            //    else if (item.priority == IDAL.DO.Priorities.fast)
            //        fast.Add(item);
            //    else
            //        normal.Add(item);
            //}
            //bestParcel = theBestParcel(droneToList, emergency);
            //if (bestParcel.Id == -1)
            //    bestParcel = theBestParcel(droneToList, fast);
            //if (bestParcel.Id == -1)
            //    bestParcel = theBestParcel(droneToList, normal);
            //if (bestParcel.Id == -1)
            //    throw new NotImplementedException();

            bestParcel = step1(droneToList, parcels);
            droneToLists[droneFind].DroneStatuses = DroneStatuses.busy;

            bestParcel.DroneId = IdDrone;
            bestParcel.Scheduled = DateTime.Now;
            dal.removeParcel(bestParcel.Id);
            dal.addParsel(bestParcel);

            return true;
            //parcels.Sort(delegate (IDAL.DO.Parcel a, IDAL.DO.Parcel b)
            //{
            //    return a.priority.CompareTo(b.priority);
            //});
            //foreach (IDAL.DO.Parcel item in parcels)
            //{
            //    do
            //    {
            //        if (item.weight < drone.MaxWeight)
            //        {
            //            IDAL.DO.Customer customer = (IDAL.DO.Customer)dal.getCustomer(item.SenderId);//לבדוק לגבי ההמרה
            //            location location = new() { latitude = customer.lattitude, longitude = customer.longitude };

            //            double distanceTest = d.DistanceBetweenPlaces(droneToList.location, location);
            //            if (distanceTest < distance)
            //                distance = distanceTest;
            //        }
            //    } while (item.priority==i);

            //}

        }
        //public IDAL.DO.Parcel theBestParcel(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        //{
        //    IDAL.DO.Customer sander;
        //    location sanderLocation = new();
        //    double distance = double.MaxValue;
        //    double weight;
        //    double batteryToTheDelivery;
        //    bool action = true;

        //    while (action && parcels.Count > 0)
        //    {
        //        foreach (IDAL.DO.Parcel item in parcels)//לבדוק אופציה של להגדיר פונקציה טמפלט של הnerste station 
        //        {
        //            if (item.weight < (IDAL.DO.WeightCategories)droneToList.MaxWeight)
        //            {
        //                sander = (IDAL.DO.Customer)dal.getCustomer(item.SenderId);//לבדוק לגבי ההמרה
        //                sanderLocation.latitude = sander.lattitude;
        //                sanderLocation.longitude = sander.longitude;

        //                double distanceTest = d.DistanceBetweenPlaces(droneToList.location, sanderLocation);
        //                if (distanceTest < distance)
        //                {
        //                    distance = distanceTest;
        //                    closeParcel = item;
        //                }
        //            }
        //        }
        //        IDAL.DO.Customer target = (IDAL.DO.Customer)dal.getCustomer(closeParcel.TargetId);
        //        location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };
        //        if (closeParcel.weight == IDAL.DO.WeightCategories.easy)
        //            weight = easy;
        //        else if (closeParcel.weight == IDAL.DO.WeightCategories.medium)
        //            weight = medium;
        //        else
        //            weight = Heavy;
        //        batteryToTheDelivery = distance * available +
        //            (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight) +
        //            (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);

        //        if (droneToList.battery < batteryToTheDelivery)
        //            parcels.Remove(closeParcel);
        //        else
        //            action = false;
        //    }
        //    if (parcels.Count <= 0)
        //        closeParcel.Id = -1;
        //    return closeParcel;

        //}

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
            droneToLists[index].DroneStatuses =  DroneStatuses.available;

            dal.removeParcel(parcelsOfDrone.Id);
            parcelsOfDrone.Delivered = DateTime.Now;
            dal.addParsel(parcelsOfDrone);

            return true;
        }

        public IDAL.DO.Parcel step1(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {
            List<IDAL.DO.Parcel> emergency = new();
            List<IDAL.DO.Parcel> fast = new();
            List<IDAL.DO.Parcel> normal = new();

            foreach (IDAL.DO.Parcel item in parcels)
            {
                if (item.weight < (IDAL.DO.WeightCategories)droneToList.MaxWeight)
                {

                    IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(item.SenderId);
                    Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                    IDAL.DO.Customer target = (IDAL.DO.Customer)dal.getCustomer(closeParcel.TargetId);
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
                    double batteryToTheDelivery = d.DistanceBetweenPlaces(droneToList.Location, sanderLocation) * available +
                        (d.DistanceBetweenPlaces(sanderLocation, targetLocation) * weight) +
                        (d.DistanceBetweenPlaces(targetLocation, TheLocationForTheNearestStation(targetLocation, stations)) * available);

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
            return step2(droneToList,(emergency.Count > 0) ? emergency : (fast.Count > 0) ? fast : normal);
        }

        public IDAL.DO.Parcel step2(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
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
            return TheNearestParcel(droneToList, (heavy.Count > 0) ? heavy : (medium.Count > 0) ? medium : easy);
        }

        public IDAL.DO.Parcel TheNearestParcel(DroneToList droneToList, List<IDAL.DO.Parcel> parcels)
        {
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
        }
    }
}
