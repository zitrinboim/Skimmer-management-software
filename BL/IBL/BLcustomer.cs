using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;

namespace BL
{
    public partial class BL : IBL
    {
        public bool addCustomer(Customer customer)
        {
            try
            {
                DO.Customer dalCustomer = new()
                {
                    Id = customer.Id,
                    name = customer.name,
                    phone = customer.phone,
                    lattitude = customer.location.latitude,
                    longitude = customer.location.longitude
                };
                _ = dal.addCustomer(dalCustomer);
                return true;
            }
            catch (DO.IdExistExeptions Ex)
            {
                throw new IdExistExeptions("ERORR" + Ex);
            }
        }

        public bool updateCustomerData(int IdCustomer, string newName, string newPhone)
        {
            try
            {
                DO.Customer tempCustomer = dal.getCustomer(IdCustomer);
                if (newName != "X" && newName != "x")
                    tempCustomer.name = newName;
                if (newPhone != "X" && newPhone != "x")
                    tempCustomer.phone = newPhone;
                _ = dal.removeCustomer(IdCustomer);
                _ = dal.addCustomer(tempCustomer);
                return true;
            }
            catch (DO.IdExistExeptions Ex)
            {
                throw new IdExistExeptions("ERORR" + Ex);
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR" + Ex);
            }
        }

        public CustomerInParcel GetCustomerInParcel(int customerId)
        {
            try
            {
                DO.Customer customer = dal.getCustomer(customerId);
                CustomerInParcel customerInParcel = new() { Id = customer.Id, name = customer.name };
                return customerInParcel;
            }
            catch (DO.IdNotExistExeptions ex)
            {
                throw new IdNotExistExeptions("Error: " + ex);
            }
        }
        public Customer GetCustomer(int customerId)
        {
            try
            {
                DO.Customer dalCustomer = dal.getCustomer(customerId);

                Customer customer = new()
                {
                    Id = dalCustomer.Id,
                    name = dalCustomer.name,
                    phone = dalCustomer.phone,
                    location = new() { latitude = dalCustomer.lattitude, longitude = dalCustomer.longitude },
                    fromCustomer = new(),
                    toCustomer = new()
                };

                List<DO.Parcel> sanderParcels = dal.DisplaysIistOfparcels(i => i.SenderId == customerId).ToList();
                foreach (DO.Parcel item in sanderParcels)
                {
                    ParcelInCustomer parcelInCustomer = new();
                    parcelInCustomer = GetParcelInCustomer(item.Id, customerId);
                    customer.fromCustomer.Add(parcelInCustomer);
                }

                List<DO.Parcel> targetParcels = dal.DisplaysIistOfparcels(i => i.TargetId == customerId).ToList();
                foreach (DO.Parcel item in targetParcels)
                {
                    ParcelInCustomer parcelInCustomer = new();
                    parcelInCustomer = GetParcelInCustomer(item.Id, customerId);
                    customer.toCustomer.Add(parcelInCustomer);
                }
                return customer;
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR" + Ex);
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
                customerToList.packagesNotYetDelivered = packagesNotYetDelivered.Count;

                List<ParcelInCustomer> receivedPackages = customer.fromCustomer.FindAll(i => i.parcelStatus == parcelStatus.Provided);
                customerToList.receivedPackages = receivedPackages.Count;

                return customerToList;
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR" + Ex);
            }
        }
        public IEnumerable<CustomerToList> DisplaysIistOfCustomers(Predicate<CustomerToList> p = null)
        {
            try
            {
                List<CustomerToList> customerToLists = new();

                List<DO.Customer> customers = dal.DisplaysIistOfCustomers().ToList();
                foreach (DO.Customer item in customers)
                {
                    customerToLists.Add(GetCustomerToList(item.Id));
                }
                return customerToLists.Where(d => p == null ? true : p(d)).ToList();
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR" + Ex);
            }
        }
    }
}
