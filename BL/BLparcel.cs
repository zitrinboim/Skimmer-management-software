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
            int addParcel = dal.addParsel(parcel);

            if (addParcel <= 0)
                throw new NotImplementedException();
            return addParcel;
        }
        public Parcel GetParcel(int parcelId)
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

            parcel.droneInParcel = GetDroneInParcel(dalParcel.DroneId);

            return parcel;
        }
        public PackageInTransfer GetPackageInTransfer(int parcelId)
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
        public ParcelInCustomer GetParcelInCustomer(int parcelId, int customerId)
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
    }
}
