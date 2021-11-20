﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL.BO
{
    public partial class BL : IBL
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
        public CustomerInParcel GetCustomerInParcel(int customerId)
        {
            IDAL.DO.Customer customer = (IDAL.DO.Customer)dal.getCustomer(customerId);
            CustomerInParcel customerInParcel = new() { Id = customer.Id, name = customer.name };
            return customerInParcel;
        }
        public Customer GetCustomer(int customerId)
        {
            IDAL.DO.Customer dalCustomer = (IDAL.DO.Customer)dal.getCustomer(customerId);

            Customer customer = new()
            {
                Id = dalCustomer.Id,
                name = dalCustomer.name,
                phone = dalCustomer.phone,
                location = new() { latitude = dalCustomer.lattitude, longitude = dalCustomer.longitude }
            };

            List<IDAL.DO.Parcel> sanderParcels = dal.DisplaysIistOfparcels(i => i.SenderId == customerId).ToList();
            foreach (IDAL.DO.Parcel item in sanderParcels)
            {
                ParcelInCustomer parcelInCustomer = GetParcelInCustomer(item.Id, customerId);
                customer.fromCustomer.Add(parcelInCustomer);
            }

            List<IDAL.DO.Parcel> targetParcels = dal.DisplaysIistOfparcels(i => i.TargetId == customerId).ToList();
            foreach (IDAL.DO.Parcel item in targetParcels)
            {
                ParcelInCustomer parcelInCustomer = GetParcelInCustomer(item.Id, customerId);
                customer.toCustomer.Add(parcelInCustomer);
            }
            return customer;
        }
    }
}
