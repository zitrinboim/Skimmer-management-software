using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



    namespace IDAL
    {
        namespace DO
        {
            public struct parcel
            {
                public int Id;
                //צריך לבדוק לגבי השדות של השולח והמקבל
                public int SenderId;
                public int TargetId;
                public WeightCategories weight;
                public Priorities priority;
                public DateTime Requested;
                public int DroneId;
                public DateTime Scheduled;
                public DateTime PickedUp;
                public DateTime Delivered;
            }
        }
    }

