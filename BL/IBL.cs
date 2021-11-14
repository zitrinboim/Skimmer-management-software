using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public interface IBL
    {
        bool addStation(Station station);
        bool addDrone(Drone drone, int idStation = 0);//לעדכן את המיקום של הרחפן להתחנה אם קיימת וכל הנגזר מזה
        bool addCustomer(Customer customer);

    }
}
