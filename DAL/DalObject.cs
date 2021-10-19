using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public DalObject() => DataSurce.Initialize();
        public void addStation()
        {
            Station temp;
            Console.WriteLine("enter name to the station");
            temp.name=Console.ReadLine();
            Console.WriteLine("enter longitudes of the station ");
            temp.longitude = Console.Read();
            Console.WriteLine("enter Latitudes of the station ");
            temp.lattitude=Console.Read();
            Console.WriteLine("enter number of CargeSlots");

        }



    }
}
