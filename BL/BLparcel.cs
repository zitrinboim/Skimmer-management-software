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
        public ParcelInCustomer GetParcelInCustomer(int parcelId)
        {
            IDAL.DO.Parcel parcel = (IDAL.DO.Parcel)dal.getParcel(parcelId);

            ParcelInCustomer parcelInCustomer = new()
            {
                Id = parcel.Id,
                weight = (WeightCategories)parcel.weight,
                priority = (Priorities)parcel.priority, 
                 
            };
            switch (parcel)
            {
                case parcel.Scheduled=DateTime.MinValue:
                    parcelInCustomer.parcelStatus = parcelStatus.defined;
                    break;
                case IDAL.DO.WeightCategories.medium:
                    weight = medium;
                    break;
                case IDAL.DO.WeightCategories.heavy:
                    weight = Heavy;
                    break;
                default:
                    break;
            }
        }
    }
}
