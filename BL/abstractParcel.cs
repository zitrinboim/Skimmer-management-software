using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL.BO
{
    public class abstractParcel
    {
        public int Id { get; set; }
        public WeightCategories weight { get; set; }
        public Priorities priority { get; set; }
        public override string ToString()
        {
            return string.Format("abstractParcel\n" + "Id {0}: weight {1}: priority{2}", Id, weight, priority);
        }
    }

}
