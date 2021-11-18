using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList : abstractParcel
    {
        public string sanderName { get; set; }
        public string targetName { get; set; }
        public parcelStatus parcelStatus { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "sanderName {0} : targetName {1}", sanderName, targetName);
        }
    }
}

