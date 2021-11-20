﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public partial class BL : IBL
    {
        /// <summary>
        /// The function returns the location of the station closest to the drone.
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
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
        /// <summary>
        /// The function returns the closest station to the drone.
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This function allows the user to add a base station to the list.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public bool addStation(Station station)//לזכור לעשות try catch 
        {
            try
            {
                IDAL.DO.Station dalStation = new()
                {
                    Id = station.Id,
                    name = station.name,
                    freeChargeSlots = station.freeChargeSlots,
                    lattitude = station.location.latitude,
                    longitude = station.location.longitude
                };
                stations.Add(dalStation);
                bool test = dal.addStation(dalStation);
                if (test)
                    return true;
                else
                    throw new NotImplementedException();
            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function updates the station data.
        /// </summary>
        /// <param name="Idstation"></param>
        /// <param name="newName"></param>
        /// <param name="ChargingSlots"></param>
        /// <returns></returns>
        public bool updateStationData(int Idstation, string newName, int ChargingSlots)
        {
            try
            {
                IDAL.DO.Station tempStation = (IDAL.DO.Station)dal.getStation(Idstation);
                dal.removeStation(Idstation);
                //Instructions for the user If there are no updates to insert an X, the test includes a mode of replacing a large X with a small one.
                if (newName != "X" && newName != "x")
                    tempStation.name = newName;
                Station station = GetStation(Idstation);
                if (ChargingSlots >= station.droneInCargeings.Count)
                    tempStation.freeChargeSlots = ChargingSlots - station.droneInCargeings.Count;
                else
                    throw new NotImplementedException();

                bool test = dal.addStation(tempStation);//הנחתי שהבוליאניות היא רק לגבי ההוספה חזרה
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
        /// This function returns the logical entity station.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            try
            {
                IDAL.DO.Station dalStation = (IDAL.DO.Station)dal.getStation(stationId);

                Location location = new() { latitude = dalStation.lattitude, longitude = dalStation.longitude };
                Station station = new() { Id = dalStation.Id, name = dalStation.name, freeChargeSlots = dalStation.freeChargeSlots, location = location };
                foreach (DroneToList item in droneToLists)
                {
                    if (item.Location == station.location)
                        station.droneInCargeings.Add(new() { Id = item.Id, battery = item.battery });
                }
                return station;

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
        /// This function returns the logical entity StationToList.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
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
                if (station.droneInCargeings==null)
                    stationToList.busyChargeSlots = 0;
                else
                    stationToList.busyChargeSlots = station.droneInCargeings.Count;
            return stationToList;
        }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
    }
}
/// <summary>
/// Displays the list of all the drones.
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
public IEnumerable<StationToList> DisplaysIistOfStations(Predicate<StationToList> p = null)
{
    List<StationToList> stationToLists = new();

    List<IDAL.DO.Station> stations = dal.DisplaysIistOfStations().ToList();
    foreach (IDAL.DO.Station item in stations)
    {
        stationToLists.Add(GetStationToList(item.Id));
    }
    return stationToLists.Where(d => p == null ? true : p(d)).ToList();
}
       
    }
}
