using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class for parcel to list.
    /// </summary>
    public class ParcelToList : abstractParcel
    {
        public string sanderName { get; set; }
        public string targetName { get; set; }
        public parcelStatus parcelStatus { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "sanderName {0}\ttargetName {1}\n\n", sanderName, targetName);
        }
    }
}

