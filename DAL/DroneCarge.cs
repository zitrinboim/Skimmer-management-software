using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IDAL
{
    namespace DO
    {
        public struct DroneCarge
        {
            public int DroneID { get; set; }
            public int StationId { get; set; }
            public override string ToString()
            {
                return string.Format("DroneCarge\nDrone I.D{0}\t Station I.D{1}", DroneID, StationId);
            }
        }
    }
}

