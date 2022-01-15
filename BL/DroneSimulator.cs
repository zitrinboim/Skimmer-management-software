using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;
using System.Threading.Tasks;
using System.Threading;
using static BL.BL;


namespace BL
{
    internal class DroneSimulator
    {
        enum Mintance {Starting, Going, charging };
        private const double VELOCITY = 0.1;
        private const int DELAY = 1000;
        private const double TIME_STOP = DELAY/1000.0;
        private const double STOP = VELOCITY / TIME_STOP;

        BL BL;
        DroneToList droneToList;
        Action actionInSimulator;

        internal DroneSimulator(BL bL, int droneId, Action updateDrone, Func<bool> isTimeRun)
        {
             BL = bL;
            droneToList = BL.DisplaysIistOfDrons(i => i.Id == droneId).First();
            actionInSimulator = updateDrone;

            while (!isTimeRun())
            {
                switch (droneToList.DroneStatuses)
                {
                    case DroneStatuses.available:

                        try
                        {
                            lock (BL)
                            {
                                BL.AssignPackageToDrone(droneId);
                            }
                            actionInSimulator();
                            Thread.Sleep(DELAY*2);

                        }
                        catch (Exception)
                        {

                            if (droneToList.battery != 100)
                            {
                                try
                                {
                                    BL.SendDroneForCharging(droneId);
                                }
                                catch (Exception)
                                {
                                    //If the drone can not return to the station because there is no place
                                    //release a random drone from a charger to bring it home.
                                    StationToList stationToList = BL.DisplaysIistOfStations(i => i.Id == BL.GetTheIdOfCloseStation(droneId)).First();
                                    Station station = BL.GetStation(stationToList.Id);
                                    BL.ReleaseDroneFromCharging(station.droneInCargeings.First().Id, 7);
                                    BL.SendDroneForCharging(droneId);

                                }

                            }
                            else
                            {
                                Thread.Sleep(DELAY*5);
                            }
                        }
                        break;
                    case DroneStatuses.maintenance:

                        break;
                    case DroneStatuses.busy:
                        break;
                }
            }

        }

       
    }
}
