using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public partial class BL:IBL
    {
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
                lattitude = station.Location.latitude,
                longitude = station.Location.longitude
            };
            bool test = dal.addStation(dalStation);
            if (test)
                return true;
            else
                throw new NotImplementedException();//לבדוק  איזה חריגה לשים כאן.
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



    }
}
