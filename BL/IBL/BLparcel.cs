using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {
        /// <summary>
        /// This function allows the user to add a parcel to the list.
        /// </summary>
        /// <param name="sanderId"></param>
        /// <param name="targetId"></param>
        /// <param name="weightCategories"></param>
        /// <param name="priorities"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int addParsel(Parcel parcel)
        {
            try
            {
                var sanderTest = dal.DisplaysIistOfCustomers(i => i.Id == parcel.Sender.Id);
                if (sanderTest == default)
                    throw new IdNotExistExeptions("אין לקוח כזה");
                var targetTest = dal.DisplaysIistOfCustomers(i => i.Id == parcel.Target.Id);
                if (targetTest == default)
                    throw new IdNotExistExeptions("אין לקוח כזה");

                DO.Parcel parcel1 = new()
                {
                    SenderId = parcel.Sender.Id,
                    TargetId = parcel.Target.Id,
                    weight = (DO.WeightCategories)parcel.weight,
                    priority = (DO.Priorities)parcel.priority,
                    Requested = DateTime.Now
                };
                int addParcel = dal.addParsel(parcel1);

                return addParcel;
            }
            catch (DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function returns an instance of the object by ID
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {
            try
            {
                DO.Parcel dalParcel = dal.getParcel(parcelId);

                Parcel parcel = new()
                {
                    Id = dalParcel.Id,
                    weight = (WeightCategories)dalParcel.weight,
                    priority = (Priorities)dalParcel.priority,
                    Scheduled = dalParcel.Scheduled,
                    Requested = dalParcel.Requested,
                    PickedUp = dalParcel.PickedUp,
                    Delivered = dalParcel.Delivered
                };

                parcel.Sender = GetCustomerInParcel(dalParcel.SenderId);
                parcel.Target = GetCustomerInParcel(dalParcel.TargetId);
                if (dalParcel.DroneId != 0)
                    parcel.droneInParcel = GetDroneInParcel(dalParcel.DroneId);

                return parcel;
            }
            catch (DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function returns an instance of the object by ID and location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public PackageInTransfer GetPackageInTransfer(Location location, int parcelId)
        {
            try
            {
                DO.Parcel parcel = dal.getParcel(parcelId);

                DO.Customer sander = dal.getCustomer(parcel.SenderId);
                Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                DO.Customer target = dal.getCustomer(parcel.TargetId);
                Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                PackageInTransfer packageInTransfer = new()
                {
                    Id = parcel.Id,
                    priority = (Priorities)parcel.priority,
                    weight = (WeightCategories)parcel.weight
                };

                if (parcel.PickedUp == DateTime.MinValue)
                {
                    packageInTransfer.packageInTransferStatus = PackageInTransferStatus.awaitingCollection;
                    packageInTransfer.distance = distanceAlgorithm.DistanceBetweenPlaces(location, sanderLocation);
                }
                else
                {
                    packageInTransfer.packageInTransferStatus = PackageInTransferStatus.OnTheWay;
                    packageInTransfer.distance = distanceAlgorithm.DistanceBetweenPlaces(sanderLocation, targetLocation);

                }

                packageInTransfer.sander = GetCustomerInParcel(parcel.SenderId);
                packageInTransfer.target = GetCustomerInParcel(parcel.TargetId);

                packageInTransfer.targetPoint = targetLocation;
                packageInTransfer.startingPoint = sanderLocation;

                return packageInTransfer;

            }
            catch (DO.IdExistExeptions Ex)
            {
                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function returns an instance of the object by ID.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelInCustomer GetParcelInCustomer(int parcelId, int customerId)
        {
            try
            {
                DO.Parcel parcel = dal.getParcel(parcelId);

                ParcelInCustomer parcelInCustomer = new()
                {
                    Id = parcel.Id,
                    weight = (WeightCategories)parcel.weight,
                    priority = (Priorities)parcel.priority,

                };
                parcelInCustomer.parcelStatus = (parcel.Scheduled == DateTime.MinValue) ? parcelStatus.defined :
                    (parcel.PickedUp == DateTime.MinValue) ? parcelStatus.associated :
                    (parcel.Delivered == DateTime.MinValue) ? parcelStatus.collected : parcelStatus.Provided;

                parcelInCustomer.CustomerInParcel = GetCustomerInParcel((parcel.SenderId == customerId) ? parcel.TargetId : parcel.SenderId);

                return parcelInCustomer;
            }
            catch (DO.IdExistExeptions Ex)
            {
                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// This function deletes an instance of a package.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool remuveParcel(int parcelId)
        {
            try
            {
                Parcel parcel = GetParcel(parcelId);
                if (parcel.Scheduled == DateTime.MinValue)
                {
                    return dal.removeParcel(parcelId);
                }
                return false;
            }

            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
            }

        }
        /// <summary>
        /// This function returns an instance of the object by ID.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelToList GetParcelToList(int parcelId)
        {
            try
            {
                Parcel parcel = GetParcel(parcelId);

                ParcelToList parcelToList = new()
                {
                    Id = parcel.Id,
                    priority = parcel.priority,
                    weight = parcel.weight,
                    sanderName = parcel.Sender.name,
                    targetName = parcel.Target.name
                };
                parcelToList.parcelStatus = (parcel.Scheduled == DateTime.MinValue) ? parcelStatus.defined :
                    (parcel.PickedUp == DateTime.MinValue) ? parcelStatus.associated :
                    (parcel.Delivered == DateTime.MinValue) ? parcelStatus.collected : parcelStatus.Provided;

                return parcelToList;
            }
            catch (DO.IdNotExistExeptions Ex)
            {
                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        /// <summary>
        /// Displays the list of all the parcels.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> DisplaysIistOfparcels(Predicate<ParcelToList> p = null)
        {
            List<ParcelToList> parcelToLists = new();

            List<DO.Parcel> parcels = dal.DisplaysIistOfparcels().ToList();
            foreach (DO.Parcel item in parcels)
            {
                parcelToLists.Add(GetParcelToList(item.Id));
            }
            return parcelToLists.Where(d => p == null ? true : p(d)).ToList();
        }
    }
}
