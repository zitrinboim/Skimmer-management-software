//using IBL.BO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleUI_BL
//{
//    internal class Display
//    {
//        IBL.BO.BL BLProgram;
//        public Display(IBL.BO.BL _bLProgram)
//        {
//            BLProgram = _bLProgram;
//        }

//        public enum enumDisplayOptions { EXIT = 0, STATION_DISPLAY, DRONE_DISPLAY, CUSTOMER_DISPLAY, PARCEL_DISPLAY, };
//        public void DisplayOptions()
//        {
//            Console.WriteLine("enter 1 to  get the information on station");
//            Console.WriteLine("enter 2 to  get the information on drone");
//            Console.WriteLine("enter 3 to  get the information on customer");
//            Console.WriteLine("enter 4 to  get the information on parcel");
//            Console.WriteLine("enter 0 to EXIT");

//            enumDisplayOptions enumUpdating;
//            int choice;
//            int.TryParse(Console.ReadLine(), out choice);
//            enumUpdating = (enumDisplayOptions)choice;
//            switch (enumUpdating)
//            {
//                case enumDisplayOptions.STATION_DISPLAY:
//                    STATION_DISPLAY();
//                    break;
//                case enumDisplayOptions.DRONE_DISPLAY:
//                    DRONE_DISPLAY();
//                    break;
//                case enumDisplayOptions.CUSTOMER_DISPLAY:
//                    CUSTOMER_DISPLAY();
//                    break;
//                case enumDisplayOptions.PARCEL_DISPLAY:
//                    PARCEL_DISPLAY();
//                    break;
//                case enumDisplayOptions.EXIT:
//                    return;
//                default:
//                    Console.WriteLine("ERROR!");
//                    break;
//            }
//        }
//        /// <summary>
//        /// The function provides information about a requested station.
//        /// </summary>
//        public void STATION_DISPLAY()
//        {
//            try
//            {
//                Console.WriteLine("enter ID number of the ststion");
//                int IdStation;
//                int.TryParse(Console.ReadLine(), out IdStation);
//                Station station = BLProgram.GetStation(IdStation);
//                Console.WriteLine(station);
//            }
//            catch (IdNotExistExeptions ex)
//            {

//                Console.WriteLine(ex);
//            }

//        }
//        /// <summary>
//        /// The function provides information about a requested drone
//        /// </summary>
//        public void DRONE_DISPLAY()
//        {
//            try
//            {
//                Console.WriteLine("enter ID number of the drone");
//                int IdDrone;
//                int.TryParse(Console.ReadLine(), out IdDrone);
//                Drone drone = BLProgram.GetDrone(IdDrone);

//                Console.WriteLine(drone);
//            }
//            catch (IdNotExistExeptions ex)
//            {

//                Console.WriteLine(ex);
//            }
//        }
//        /// <summary>
//        /// The function provides information about a requested customer.
//        /// </summary>
//        public void CUSTOMER_DISPLAY()
//        {
//            try
//            {
//                Console.WriteLine("enter ID number of the customer");
//                int IdCustomer;
//                int.TryParse(Console.ReadLine(), out IdCustomer);
//                Customer customer = BLProgram.GetCustomer(IdCustomer);
//                Console.WriteLine(customer);
//            }
//            catch (IdNotExistExeptions ex)
//            {

//                Console.WriteLine(ex);
//            }
//        }
//        /// <summary>
//        /// The function provides information about a requested parcel
//        /// </summary>
//        public void PARCEL_DISPLAY()
//        {
//            try
//            {

//                Console.WriteLine("enter ID number of the parcel");
//                int IdParcel;
//                int.TryParse(Console.ReadLine(), out IdParcel);
//                Parcel parcel = BLProgram.GetParcel(IdParcel);

//                Console.WriteLine(parcel);
//            }
//            catch (IdNotExistExeptions ex)
//            {

//                Console.WriteLine(ex);
//            }
//        }
//    }
//}
