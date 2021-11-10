using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BL
{
       public class StationBlObject  
        {
       // DalObject dalProgram = new DalObject();
        public bool AddStation(Station station)
        {
            Station myStation = new()
            {
                Id = station.Id,
                name = station.name,
                //longitude = longitude,
                //lattitude = lattitude,
                freeChargeSlots = station.freeChargeSlots
            };
            bool test = dalProgram.addStation(station);
            if (test)
                Console.WriteLine("the transaction completed successfully");
            else
                Console.WriteLine("ERROR");


            // int find = dalProgram.stations.FindIndex(Station => Station.Id == station.Id);
            return false;
    }
     

        
        }
    
}
