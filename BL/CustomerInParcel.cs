using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return string.Format("Customer\n Id {0}: mane {1}: ", Id, name);
        }
    }
}

