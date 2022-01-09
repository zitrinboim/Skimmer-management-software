using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace BO
    {
        public class Drone : abstractDrone
        {
            public PackageInTransfer packageInTransfer { get; set; }
            public override string ToString()
            {
                return string.Format(base.ToString() + "packageInTransfer {0}\n", packageInTransfer);
            }
        }
    }

