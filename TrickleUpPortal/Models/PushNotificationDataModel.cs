using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrickleUpPortal.Models
{
    public class PushNotificationDataModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int CropId { get; set; }
        public int StepId { get; set; }
        public int LangCode { get; set; }
        public string VideoURL { get; set; }
        public string CropName { get; set; }
        public string StepName { get; set; }
        public string StepImageURL { get; set; }
        public string CreatedOn { get; set; }
        public string ResponseMessage { get; set; }
    }
}