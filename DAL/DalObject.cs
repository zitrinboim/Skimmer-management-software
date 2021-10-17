using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DalObject
{
    static class DataSurce
    {
        internal static Drone[] drones = new Drone[10];
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static parcel[] parcels = new parcel[1000];
        //internal static List<Drone> drones = new List<Drone>();

        internal static class Config
        {
            internal static int DroneIndex = 0;
            internal static int StationIndex = 0;
            internal static int CustomerIndex = 0;
            internal static int ParcelIndex = 0;

            internal static int DroneIdRun = 0;
            internal static int StationIdRun = 0;

        }
        public static void Initialize()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 5; i++)
            {
                drones[Config.DroneIndex++] = new Drone()
                {
                    Id = Config.DroneIdRun++,
                    Model = "F40",
                    MaxWeight = (WeightCategories)random.Next(1, 4),
                    battery = (double)random.Next(0, 100),
                    Status = (DroneStatuses)random.Next(1, 4)
                };
            }


            stations[Config.StationIndex++] = new Station()
            {
                ChargeSlots = random.Next(0, 10),
                Id = Config.StationIdRun++,
                name = "central Station",
                longitude= 34.748340,
                lattitude
            };
            
        }
        
        

    }
}
    

      
            

        




