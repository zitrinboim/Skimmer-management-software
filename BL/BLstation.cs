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
            IDAL.DO.Station dalStation = new()
            {
                Id = station.Id,
                name = station.name,
                freeChargeSlots = station.freeChargeSlots,
                lattitude = station.Location.latitude,
                longitude = station.Location.longitude
            };
            bool test = dal.addStation(dalStation);
            if (test)
                return true;
            else
                throw new NotImplementedException();//לבדוק  איזה חריגה לשים כאן.
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
            IDAL.DO.Station tempStation = (IDAL.DO.Station)dal.getStation(Idstation);//לבדוק לגבי ההמרה
            dal.removeStation(Idstation);
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
        /// <summary>
        /// This function returns the logical entity station.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            IDAL.DO.Station dalStation = (IDAL.DO.Station)dal.getStation(stationId);
            if (dalStation.Id == 0)
                throw new NotImplementedException();//////////////////////////////////////////////
            Location location = new() { latitude = dalStation.lattitude, longitude = dalStation.longitude };
            Station station = new() { Id = dalStation.Id, name = dalStation.name, freeChargeSlots = dalStation.freeChargeSlots, location = location };
            foreach (DroneToList item in droneToLists)
            {
                if (item.Location == station.location)
                    station.droneInCargeings.Add(new() { Id = item.Id, battery = item.battery });
            }

            return station;
        }
    }
}
