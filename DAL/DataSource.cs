using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public static class DataSource
    {
        //Create lists of program objects
        internal static List<Drone> drones = new List<Drone>();
        internal static List<Station> stations = new List<Station>();
        internal static List<Customer> customers = new List<Customer>();
        internal static List<DroneCarge> droneCarges = new List<DroneCarge>();
        internal static List<Parcel> parcels = new List<Parcel>();

        internal static string[] names = new string[] { "Reuben", "Simeon", "Levi", "Judah", "Issachar", "Zebulun", "Dan", "Naphtali", "Gad", "Asher", };
        internal static string[] phoneNumbers = new string[] { "050-4176977", "052-7184790", "058-4423540", "050-4106067", "052-7636475", };
        
        internal static class Config//Runner number for package IDs
        {
            internal static int ParcelIdRun = 1000;
            internal static double available=0.0;
            internal static double easy;
            internal static double medium;
            internal static double Heavy;
            internal static double ChargingRate;
        }
        /// <summary>
        /// Function for filling the lists with random values for checking the correctness of the data
        /// </summary>
        public static void Initialize()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 5; i++)
            {
                drones.Add(new Drone()
                {
                    Id = random.Next(1, 100),
                    Model = "F40",
                    MaxWeight = (WeightCategories)random.Next(1, 4),
                });
            }

            stations.Add(new Station()
            {
                Id = random.Next(1, 50),
                name = "central Station TLV",
                longitude = 32.056811,
                lattitude = 34.779302,
                freeChargeSlots = random.Next(1, 10)
            });

            stations.Add(new Station()
            {
                Id = random.Next(1, 50),
                name = "central Station Jrusalem",
                longitude = 31.788729,
                lattitude = 35.202984,
                freeChargeSlots = random.Next(1, 10)

            });
            for (int i = 0; i < 10; i++)
            {
                customers.Add(new Customer()
                {
                    Id = random.Next(200000000, 399999999),
                    name = names[random.Next(0, 10)],
                    phone = phoneNumbers[random.Next(0, 4)],
                    longitude = 32 + random.NextDouble(),
                    lattitude = 35 + random.NextDouble()
                });
            }
            for (int i = 0; i < 10; i++)
            {
                parcels.Add(new Parcel()
                {
                    Id = Config.ParcelIdRun++,
                    SenderId = customers[random.Next(0, 10)].Id,
                    TargetId = customers[random.Next(0, 10)].Id,
                    weight = (WeightCategories)random.Next(1, 4),
                    priority = (Priorities)random.Next(1, 4),
                    DroneId = 0,
                    Requested = DateTime.Now.AddHours(random.Next(0, 3)),
                    Scheduled = DateTime.Now.AddHours(random.Next(5, 8)),
                    PickedUp = DateTime.Now.AddDays(1),
                    Delivered = DateTime.Now.AddDays(1).AddHours(2)
                });
            }
        }
    }
}











