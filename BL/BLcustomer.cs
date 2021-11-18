using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public partial class BL:IBL
    {
        public bool addCustomer(Customer customer)
        {
            IDAL.DO.Customer dalCustomer = new()
            {
                Id = customer.Id,
                name = customer.name,
                phone = customer.phone,
                lattitude = customer.location.latitude,
                longitude = customer.location.longitude
            };
            bool test = dal.addCustomer(dalCustomer);
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }
        public bool updateCustomerData(int IdCustomer, string newName, string newPhone)
        {
            IDAL.DO.Customer tempCustomer = (IDAL.DO.Customer)dal.getCustomer(IdCustomer);//לבדוק לגבי ההמרה
            dal.removeCustomer(IdCustomer);
            if (newName != "X" && newName != "x")
                tempCustomer.name = newName;
            if (newPhone != "X" && newPhone != "x")
                tempCustomer.phone = newPhone;
            bool test = dal.addCustomer(tempCustomer);//הנחתי שהבוליאניות היא רק לגבי ההוספה חזרה
            if (test)
                return true;
            else
                throw new NotImplementedException();
        }
    }
}
