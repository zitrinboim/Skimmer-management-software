using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public partial class BL:IBL
    {
        public int addParsel(int sanderId, int targetId, WeightCategories weightCategories, Priorities priorities)
        {
            IDAL.DO.Parcel dalParcel = new()
            {
                Id = 0,
                SenderId = sanderId,
                TargetId = targetId,
                weight = (IDAL.DO.WeightCategories)weightCategories,
                priority = (IDAL.DO.Priorities)priorities,
                DroneId = 0,
                Requested = DateTime.Now,
                Delivered = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Scheduled = DateTime.MinValue
            };
            int addParcel = dal.addParsel(dalParcel);
            if (addParcel < 0)
                throw new NotImplementedException();
            return addParcel;

        }
    }
}
