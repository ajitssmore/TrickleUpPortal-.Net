using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrickleUpPortal.Models
{
    public class NotificationModel
    {
        public Nullable<int> contextId { get; set; }
        public Nullable<int> districtId { get; set; }
        public Nullable<int> grampanchayatId { get; set; }
        public Nullable<int> languageId { get; set; }
        public string notificationContext { get; set; }
        public Nullable<int> stateId { get; set; }
        public List<int> villageIdList { get; set; }
        public Boolean Active { get; set; }
        public string category { get; set; }
    }
}