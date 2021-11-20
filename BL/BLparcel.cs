using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
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
        public int addParsel(IDAL.DO.Parcel parcel)
        {
            try
            {
                int addParcel = dal.addParsel(parcel);

                if (addParcel <= 0)
                    throw new NotImplementedException();
                return addParcel;
            }
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
        }
        public Parcel GetParcel(int parcelId)
        {
            try
            {
                IDAL.DO.Parcel dalParcel = (IDAL.DO.Parcel)dal.getParcel(parcelId);

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
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public PackageInTransfer GetPackageInTransfer(int parcelId)
        {
            try
            {
                IDAL.DO.Parcel parcel = (IDAL.DO.Parcel)dal.getParcel(parcelId);

                if (parcel.Scheduled == DateTime.MinValue || parcel.Delivered != DateTime.MinValue)
                    throw new NotImplementedException();//כי אין חבילה כזו בהעברה.

                IDAL.DO.Customer sander = (IDAL.DO.Customer)dal.getCustomer(parcel.SenderId);
                Location sanderLocation = new() { latitude = sander.lattitude, longitude = sander.longitude };

                IDAL.DO.Customer target = (IDAL.DO.Customer)dal.getCustomer(parcel.TargetId);
                Location targetLocation = new() { latitude = target.lattitude, longitude = target.longitude };

                PackageInTransfer packageInTransfer = new()
                {
                    Id = parcel.Id,
                    priority = (Priorities)parcel.priority,
                    weight = (WeightCategories)parcel.weight
                };

                if (parcel.PickedUp == DateTime.MinValue)
                    packageInTransfer.packageInTransferStatus = PackageInTransferStatus.awaitingCollection;
                else
                    packageInTransfer.packageInTransferStatus = PackageInTransferStatus.OnTheWay;

                packageInTransfer.sander = GetCustomerInParcel(parcel.SenderId);
                packageInTransfer.target = GetCustomerInParcel(parcel.TargetId);

                packageInTransfer.targetPoint = targetLocation;
                packageInTransfer.startingPoint = sanderLocation;
                packageInTransfer.distance = d.DistanceBetweenPlaces(sanderLocation, targetLocation);

                return packageInTransfer;

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
        public ParcelInCustomer GetParcelInCustomer(int parcelId, int customerId)
        {
            try
            {
                IDAL.DO.Parcel parcel = (IDAL.DO.Parcel)dal.getParcel(parcelId);

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
            catch (IDAL.DO.IdExistExeptions Ex)
            {

                throw new IdExistExeptions("ERORR", Ex);
            }
            catch (IDAL.DO.IdNotExistExeptions Ex)
            {

                throw new IdNotExistExeptions("ERORR", Ex);
            }
        }
        public ParcelToList GetParcelToList(int parcelId)
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
        /// <summary>
        /// Displays the list of all the drones.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<ParcelToList> DisplaysIistOfparcels(Predicate<ParcelToList> p = null)
        {
            List<ParcelToList> parcelToLists = new();

            List<IDAL.DO.Parcel> parcels = dal.DisplaysIistOfparcels().ToList();
            foreach (IDAL.DO.Station item in stations)
            {
                parcelToLists.Add(GetParcelToList(item.Id));
            }
            return parcelToLists.Where(d => p == null ? true : p(d)).ToList();
        }
    }
}
