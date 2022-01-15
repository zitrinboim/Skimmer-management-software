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
        /// The function returns the location of the station closest to the drone.
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        private Location TheLocationForTheNearestStation(Location customerLocation, List<DO.Station> stations)
        {
            DO.Station closeStation = stations[0];
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
        /// <summary>
        /// The function returns the closest station to the drone.
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        private DO.Station TheNearestStation(Location customerLocation, List<DO.Station> stations)
        {
            DO.Station closeStation = stations[0];
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
        /// <summary>
        /// This function allows the user to add a base station to the list.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool addStation(Station station)//לזכור לעשות try catch 
        {
            try
            {
                DO.Station dalStation = new()
                {
                    Id = station.Id,
                    name = station.name,
                    freeChargeSlots = station.freeChargeSlots,
                    lattitude = station.location.latitude,
                    longitude = station.location.longitude
                };

                stations.Add(dalStation);
                bool test = dal.addStation(dalStation);
                return test ? true : false;
            }
            catch (DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR" + Ex);
            }
        }
        /// <summary>
        /// This function updates the station data.
        /// </summary>
        /// <param name="Idstation"></param>
        /// <param name="newName"></param>
        /// <param name="ChargingSlots"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool updateStationData(int Idstation, string newName, int ChargingSlots)
        {
            try
            {
                DO.Station tempStation = dal.getStation(Idstation);
                //Instructions for the user If there are no updates to insert an X, the test includes a mode of replacing a large X with a small one.
                if (newName != "X" && newName != "x")
                    tempStation.name = newName;
                Station station = GetStation(Idstation);
                if (ChargingSlots >= station.droneInCargeings.Count)
                    tempStation.freeChargeSlots = ChargingSlots - station.droneInCargeings.Count;
                else
                    throw new invalidValueForChargeSlots("המספר שהכנסת אינו תקין יש לבחור מספר העולה על כמות הרחפנים הנטענים כרגע בתחנה");
                dal.removeStation(Idstation);
              _ = dal.addStation(tempStation);
                return true;
            }
            catch (DO.IdExistExeptions Ex)
            {
                throw new IdExistExeptions("ERORR" + Ex);
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR" + Ex);
            }
           
        }
        /// <summary>
        /// This function returns the logical entity station.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            try
            {
                DO.Station dalStation = dal.getStation(stationId);

                Station station = new()
                {
                    Id = dalStation.Id,
                    name = dalStation.name,
                    freeChargeSlots = dalStation.freeChargeSlots,
                    location = new() { latitude = dalStation.lattitude, longitude = dalStation.longitude },
                    droneInCargeings = new()
                };
                foreach (DroneToList item in droneToLists)
                {
                    if (item.Location.latitude == station.location.latitude && item.Location.longitude == station.location.longitude &&
                        item.DroneStatuses == DroneStatuses.maintenance)
                        station.droneInCargeings.Add(new() { Id = item.Id, battery = item.battery });
                }

                return station;

            }
            catch (DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR\n" + Ex);
            }
        }
        /// <summary>
        /// This function returns the logical entity StationToList.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public StationToList GetStationToList(int stationId)
        {
            try
            {
                Station station = GetStation(stationId);

                StationToList stationToList = new()
                {
                    Id = station.Id,
                    name = station.name,
                    freeChargeSlots = station.freeChargeSlots
                };
                if (station.droneInCargeings == null)
                    stationToList.busyChargeSlots = 0;
                else
                    stationToList.busyChargeSlots = station.droneInCargeings.Count;
                return stationToList;
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// Displays the list of all the drones.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> DisplaysIistOfStations(Predicate<StationToList> p = null)
        {
            try
            {
                List<StationToList> stationToLists = new();

                List<DO.Station> stations = dal.DisplaysIistOfStations().ToList();
                foreach (DO.Station item in stations)
                {
                    stationToLists.Add(GetStationToList(item.Id));
                }
                return stationToLists.Where(d => p == null ? true : p(d)).ToList();
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
    }
}
