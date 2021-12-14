using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    public class abstractParcel
    {
        public int Id { get; set; }
        public WeightCategories weight { get; set; }
        public Priorities priority { get; set; }
        public override string ToString()
        {
            return string.Format("Id {0}\tweight {1}\tpriority {2}\t", Id, weight, priority);
        }
    }

}
