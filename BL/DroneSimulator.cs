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
        enum Mintance { Starting, Going, charging };
        private const double VELOCITY = 0.1;
        private const int DELAY = 1000;
        private const double TIME_STOP = DELAY / 1000.0;
        private const double STOP = VELOCITY / TIME_STOP;

        BL BL;
        Drone drone;
        DroneToList droneToList;
        Action actionInSimulator;
        Customer sander;
        Customer target;
        double disLocationsX;
        double disLocationsY;
        int km;
        internal DroneSimulator(BL bL, int droneId, Action updateDrone, Func<bool> isTimeRun)
        {
            BL = bL;
            droneToList = BL.DisplaysIistOfDrons(i => i.Id == droneId).First();
            drone = BL.GetDrone(droneId);

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
                            Thread.Sleep(DELAY * 2);

                        }
                        catch (Exception)
                        {

                            if (droneToList.battery != 100)
                            {
                                try
                                {
                                    lock (BL)
                                    {
                                        BL.SendDroneForCharging(droneId);
                                    }
                                    actionInSimulator();
                                    Thread.Sleep(DELAY * 2);
                                }
                                catch (Exception)
                                {
                                    //If the drone can not return to the station because there is no place
                                    //release a random drone from a charger to bring it home.

                                    lock (BL)
                                    {
                                        StationToList stationToList = BL.DisplaysIistOfStations(i => i.Id == BL.GetTheIdOfCloseStation(droneId)).First();
                                        Station station = BL.GetStation(stationToList.Id);
                                        BL.ReleaseDroneFromCharging(station.droneInCargeings.First().Id, 7);
                                        BL.SendDroneForCharging(droneId);
                                        actionInSimulator();
                                        Thread.Sleep(DELAY * 2);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(DELAY * 5);
                            }
                        }
                        break;
                    case DroneStatuses.maintenance:
                        {

                            while (droneToList.battery < 100)
                            {
                                if (droneToList.battery + 3.0 < 100)
                                    droneToList.battery += 3.0;
                                else
                                    droneToList.battery = 100.0;

                                Thread.Sleep(DELAY * 3);
                                actionInSimulator();
                            }
                            lock (BL)
                            {
                                BL.ReleaseDroneFromCharging(droneId, 1);
                                actionInSimulator();
                            }
                            Thread.Sleep(DELAY * 2);
                        }
                        break;
                    case DroneStatuses.busy:
                        {
                            drone = BL.GetDrone(droneId);
                            sander = BL.GetCustomer(drone.packageInTransfer.sander.Id);
                            target = BL.GetCustomer(drone.packageInTransfer.target.Id);

                            if (drone.packageInTransfer.packageInTransferStatus == PackageInTransferStatus.awaitingCollection)
                            {
                                locationDinamic(sander);
                                lock (BL)
                                {
                                    BL.PackageCollectionByDrone(droneId);
                                    actionInSimulator();
                                }
                                Thread.Sleep(DELAY * 2);
                            }
                            else
                            {
                                locationDinamic(target);
                                lock (BL)
                                {
                                    BL.DeliveryPackageToCustomer(droneId);
                                    actionInSimulator();
                                }
                                Thread.Sleep(DELAY * 2);
                            }
                        }
                        break;
                }
            }

        }

        private void locationDinamic(Customer customer)
        {
            km = (int)drone.packageInTransfer.distance;
            disLocationsX = customer.location.longitude - drone.Location.longitude;
            disLocationsY = customer.location.latitude - drone.Location.latitude;
            for (int i = km; i > 0; i--)
            {
                droneToList.Location.longitude += (disLocationsX / km);
                droneToList.Location.latitude += (disLocationsY / km);
                actionInSimulator();
                Thread.Sleep(DELAY);

            }
        }
    }
}
