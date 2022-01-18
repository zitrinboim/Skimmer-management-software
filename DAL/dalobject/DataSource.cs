using DO;
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


        internal static class Config//Runner number for package IDs
        {
            internal static int ParcelIdRun = 1000;
            internal static double available = 0.05;
            internal static double easy = 0.2;
            internal static double medium = 0.3;
            internal static double Heavy = 0.4;
            internal static double ChargingRate = 1.0;
            internal static int password = 1111;
        }
        /// <summary>
        /// Function for filling the lists with random values for checking the correctness of the data
        /// </summary>
        public static void Initialize()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            drones.Add(new Drone()
            {
                Id = 5,
                Model = "F40",
                MaxWeight = WeightCategories.easy,
            });
            drones.Add(new Drone()
            {
                Id = 10,
                Model = "F40",
                MaxWeight = WeightCategories.heavy,
            });
            drones.Add(new Drone()
            {
                Id = 15,
                Model = "F40",
                MaxWeight = WeightCategories.medium,
            });
            drones.Add(new Drone()
            {
                Id = 20,
                Model = "F40",
                MaxWeight = WeightCategories.easy,

            });
            drones.Add(new Drone()
            {
                Id = 25,
                Model = "F40",
                MaxWeight = WeightCategories.heavy,
            });


            stations.Add(new Station()
            {
                Id = random.Next(1, 50),
                name = "תחנה מרכזית תל אביב",
                longitude = 32.056105,
                lattitude = 34.779242,
                freeChargeSlots = random.Next(1, 10)
            });

            stations.Add(new Station()
            {
                Id = random.Next(1, 50),
                name = "מבחר בני ברק",
                longitude = 32.092998,
                lattitude = 34.824273,
                freeChargeSlots = random.Next(1, 10)
            });

            customers.Add(new Customer()
            {
                Id = 205683501,
                name = "שמעון ציטרינבאום",
                phone = "052-718-4790",
                longitude = 32.115431,
                lattitude = 34.840545
            });
            customers.Add(new Customer()
            {
                Id = 311210124,
                name = "יאיר בוסו",
                phone = "058-442-3540",
                longitude = 32.085140,
                lattitude = 34.885387
            });
            customers.Add(new Customer()
            {
                Id = 206302226,
                name = "אשר אלוס",
                phone = "053-478-6278",
                longitude = 32.085638,
                lattitude = 34.881424
            });
            customers.Add(new Customer()
            {
                Id = 225352376,
                name = "יונתן שלומוב",
                phone = "050-4176977",
                longitude = 32.090177,
                lattitude = 34.825682
            });
            customers.Add(new Customer()
            {
                Id = 346945332,
                name = "יואל סלושץ",
                phone = "058-5906349",
                longitude = 32.089624,
                lattitude = 34.877801
            });
            customers.Add(new Customer()
            {
                Id = 229195465,
                name = "דב ליברמנש",
                phone = "0526705567",
                longitude = 32.068461,
                lattitude = 34.916007
            });
            customers.Add(new Customer()
            {
                Id = 312858348,
                name = "דניאל בודנשטיין",
                phone = "050-455-7765",
                longitude = 32.083335,
                lattitude = 34.878194
            });
            customers.Add(new Customer()
            {
                Id = 311980988,
                name = "מנדי יוסוביץ",
                phone = "058-342-7981",
                longitude = 32.343760,
                lattitude = 34.860743
            });

            //שמעון יאיר
            parcels.Add(new Parcel()
            {
                Id = Config.ParcelIdRun++,
                SenderId = 205683501,
                TargetId = 311210124,
                weight = WeightCategories.easy,
                priority = Priorities.emergency,
                DroneId = 0,
                Requested = DateTime.Now,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            });
            //אשר ךיונתן
            parcels.Add(new Parcel()
            {
                Id = Config.ParcelIdRun++,
                SenderId = 206302226,
                TargetId = 225352376,
                weight = WeightCategories.easy,
                priority = Priorities.emergency,
                DroneId = 0,
                Requested = DateTime.Now,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            });
            //יואל לדב
            parcels.Add(new Parcel()
            {
                Id = Config.ParcelIdRun++,
                SenderId = 346945332,
                TargetId = 229195465,
                weight = WeightCategories.easy,
                priority = Priorities.emergency,
                DroneId = 0,
                Requested = DateTime.Now,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            });
            //דניאל למנדי
            parcels.Add(new Parcel()
            {
                Id = Config.ParcelIdRun++,
                SenderId = 312858348,
                TargetId = 311980988,
                weight = WeightCategories.easy,
                priority = Priorities.emergency,
                DroneId = 0,
                Requested = DateTime.Now,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            });
        }
    }
}











