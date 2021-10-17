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

            internal static int DroneIdRun = 1;
            internal static int StationIdRun = 1;

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
                    battery = (double)random.Next(0, 101),
                    Status = (DroneStatuses)random.Next(1, 4)
                };
            }


            stations[Config.StationIndex++] = new Station()
            {
                ChargeSlots = random.Next(1, 10),
                Id = Config.StationIdRun++,
                name = "central Station TLV",
                longitude = 32.056811,
                lattitude = 34.779302
            };

            stations[Config.StationIndex++] = new Station()
            {
                ChargeSlots = random.Next(1, 10),
                Id = Config.StationIdRun++,
                name = "central Station Jrusalem",
                longitude = 31.788729,
                lattitude = 35.202984
            };
            for (int i = 0; i < 10; i++)
            {
                customers[Config.CustomerIndex++] = new Customer()
                {
                    id = random.Next(200000000, 399999999),
                    phone = "05" + random.Next(0, 9) + "-" + random.Next(1000000, 9999999),
                    name

                }
            }


        }
        class GFG
        {
            static int MAX = 26;

            // Returns a String of random alphabets of
            // length n.
            static String printRandomString(int n)
            {
                char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g',
                        'h', 'i', 'j', 'k', 'l', 'm', 'n',
                        'o', 'p', 'q', 'r', 's', 't', 'u',
                        'v', 'w', 'x', 'y', 'z' };

                Random random = new Random();
                String res = "";
                for (int i = 0; i < n; i++)
                    res = res + alphabet[(int)(random.Next(0, MAX))];

                return res;
            }

        }
}











