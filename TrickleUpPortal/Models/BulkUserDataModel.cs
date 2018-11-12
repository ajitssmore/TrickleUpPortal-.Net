using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrickleUpPortal.Models
{
    public class BulkUserDataModel
    {
        public string Active { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string Grampanchayat { get; set; }
        public string Aadhaar { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public string Language { get; set; }
        public string FCMToken { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string BulkUploadId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string ErrorMessage { get; set; }
    }
}