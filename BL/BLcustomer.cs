using System;
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
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
        }
        public bool updateCustomerData(int IdCustomer, string newName, string newPhone)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
        }

        public CustomerInParcel GetCustomerInParcel(int customerId)
        {
            try
            {
                IDAL.DO.Customer customer = (IDAL.DO.Customer)dal.getCustomer(customerId);
                CustomerInParcel customerInParcel = new() { Id = customer.Id, name = customer.name };
                return customerInParcel;
            }
            catch (IDAL.DO.IdNotExistExeptions ex)
            {
                throw new IdNotExistExeptions("Error: ", ex);
            }

        }
        public Customer GetCustomer(int customerId)
        {
            try
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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public CustomerToList GetCustomerToList(int customerID)
        {
            try
            {
                Customer customer = GetCustomer(customerID);

                CustomerToList customerToList = new()
                {
                    Id = customer.Id,
                    name = customer.name,
                    phone = customer.phone
                };
                //Finds the relevant objects by summoning a client entity that stores the requested data.
                List<ParcelInCustomer> packagesProvided = customer.toCustomer.FindAll(i => i.parcelStatus == parcelStatus.Provided);
                customerToList.packagesProvided = packagesProvided.Count;

                List<ParcelInCustomer> PackagesOnTheWay = customer.toCustomer.FindAll(i => i.parcelStatus != parcelStatus.Provided);
                customerToList.PackagesOnTheWay = PackagesOnTheWay.Count;

                List<ParcelInCustomer> packagesNotYetDelivered = customer.fromCustomer.FindAll(i => i.parcelStatus != parcelStatus.Provided);
                customerToList.PackagesOnTheWay = packagesNotYetDelivered.Count;

                List<ParcelInCustomer> receivedPackages = customer.fromCustomer.FindAll(i => i.parcelStatus == parcelStatus.Provided);
                customerToList.PackagesOnTheWay = receivedPackages.Count;

                return customerToList;
            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public IEnumerable<CustomerToList> DisplaysIistOfCustomers(Predicate<CustomerToList> p = null)
        {
            try
            {
                List<CustomerToList> customerToLists = new();

                List<IDAL.DO.Customer> customers = dal.DisplaysIistOfCustomers().ToList();
                foreach (IDAL.DO.Customer item in customers)
                {
                    customerToLists.Add(GetCustomerToList(item.Id));
                }
                return customerToLists.Where(d => p == null ? true : p(d)).ToList();

            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
    }
}
