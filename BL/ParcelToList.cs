using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList:abstractParcel
        {
            public string sanderName { get; set; }
            public string targetName { get; set; }
            public parcelStatus parcelStatus { get; set; }
        }
    }
}
