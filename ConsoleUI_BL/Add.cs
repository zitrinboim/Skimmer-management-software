using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    public class Add
    {
        BL.BL BLProgram;
        public Add(BL.BL _bLProgram)
        {
            BLProgram = _bLProgram;
        }
        public enum enumInsertOptions { EXIT = 0, ADD_STATION, ADD_DRONE, ADD_CUSTOMER, ADD_PARCEL };
        public void insertOptions()
        {

            Console.WriteLine("enter 1 to adding a base station to the list of stations");
            Console.WriteLine("enter 2 to adding a drone to the list of existing drones");
            Console.WriteLine("enter 3 to admission of a new customer to the customer list");
            Console.WriteLine("enter 4 to receipt of package for shipment");
            Console.WriteLine("enter 0 to EXIT");

            enumInsertOptions EnumInsertOptions;
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            EnumInsertOptions = (enumInsertOptions)choice;

            switch (EnumInsertOptions)
            {
                case enumInsertOptions.ADD_STATION:
                    ADD_STATION();
                    break;
                case enumInsertOptions.ADD_DRONE:
                    ADD_DRONE();
                    break;
                case enumInsertOptions.ADD_CUSTOMER:
                    ADD_CUSTOMER();
                    break;
                case enumInsertOptions.ADD_PARCEL:
                    ADD_PARCEL();
                    break;
                case enumInsertOptions.EXIT:
                    return;
                default:
                    Console.WriteLine("ERROR!");
                    break;
            }

        }
        public void ADD_STATION()
        {
            Console.WriteLine("enter id ");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("enter name of station");
            string name = Console.ReadLine();
            Console.WriteLine("enter longitude of the station ");
            double longitude;
            double.TryParse(Console.ReadLine(), out longitude);
            Console.WriteLine("enter Latitudes of the station ");
            double lattitude;
            double.TryParse(Console.ReadLine(), out lattitude);
            Location location = new() { longitude = longitude, latitude = lattitude };
            Console.WriteLine("enter number of CargeSlots");
            int ChargeSlots;
            int.TryParse(Console.ReadLine(), out ChargeSlots);
            Station station = new()
            {
                Id = id,
                name = name,
                Location = location,
                freeChargeSlots = ChargeSlots
                //רשימת הרחפנים בטעינה תאותחל לרשימה ריקה לא יודע איך לעשות את זה
            };
            //Check the integrity of the input
            bool test = BLProgram.addStation(station);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// This function picks up drone values and creates an object of this type
        /// </summary>
        public void ADD_DRONE()
        {
            WeightCategories weightCategories;
            Console.WriteLine("enter id ");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("enter model of drone");
            string model = Console.ReadLine();
            Console.WriteLine("Enter the maximum weight for this drone, insert 1 for light weight, 2 for medium weight, 3 for heavy weight. ");
            int weight;
            int.TryParse(Console.ReadLine(), out weight);
            weightCategories = (WeightCategories)weight;
            Console.WriteLine("Enter the number of the station to which you want to add the drone ");
            int idStation;
            int.TryParse(Console.ReadLine(), out idStation);
            Drone drone = new()
            {
                Id = id,
                Model = model,
                MaxWeight = weightCategories
            };
            //Check the integrity of the input
            bool test = BLProgram.addDrone(drone, idStation);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// This function picks up customer values and creates an object of this type
        /// </summary>
        public void ADD_CUSTOMER()
        {
            Console.WriteLine("enter your id ");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine("enter your name");
            string name = Console.ReadLine();
            Console.WriteLine("enter your longitude  ");
            double longitude;
            double.TryParse(Console.ReadLine(), out longitude);
            Console.WriteLine("enter your Latitudes ");
            double lattitude;
            double.TryParse(Console.ReadLine(), out lattitude);
            Location location = new() { longitude = longitude, latitude = lattitude };
            Console.WriteLine("enter your phone number");
            string phone = Console.ReadLine();
            Customer customer = new()
            {
                Id = id,
                name = name,
                phone = phone,
                location = location
            };
            //Check the integrity of the input
            bool test = BLProgram.addCustomer(customer);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");
        }
        /// <summary>
        /// This function picks up parcel values and creates an object of this type
        /// </summary>
        public void ADD_PARCEL()
        {
            WeightCategories weightCategories;
            Priorities priorities;
            Console.WriteLine("enter sander id ");
            int sanderId;
            int.TryParse(Console.ReadLine(), out sanderId);
            Console.WriteLine("enter target id ");
            int targetId;
            int.TryParse(Console.ReadLine(), out targetId);
            Console.WriteLine("Enter the maximum weight for this drone, insert 1 for light weight, 2 for medium weight, 3 for heavy weight. ");
            int weight;
            int.TryParse(Console.ReadLine(), out weight);
            weightCategories = (WeightCategories)weight;
            Console.WriteLine("Enter the urgency level of the package Insert 1 for regular delivery 2 for fast delivery 3 for urgent delivery");
            int priority;
            int.TryParse(Console.ReadLine(), out priority);
            priorities = (Priorities)priority;
            //Console.WriteLine("In a few days you will want the shipment? ");
            //int days;
            //int.TryParse(Console.ReadLine(), out days);
            
            //IDAL.DO.Parcel parcel = new()
            //{
            //    Id = 0,
            //    SenderId= sanderId,
            //    TargetId = targetId,
            //    weight = (IDAL.DO.WeightCategories)weightCategories,
            //    priority = (IDAL.DO.Priorities)priorities,
            //    DroneId = 0,
            //   // Requested = DateTime.Now.AddDays(days)
            //};
            int parcelId = BLProgram.addParsel(sanderId,targetId, weightCategories, priorities);
            Console.WriteLine("the transaction completed successfully");
        }
    }
}