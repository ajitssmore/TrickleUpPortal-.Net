using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrickleUpPortal.Models
{
    public class LiveStockPushNotificationDataModel
    {
        public string CategorySubject { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int LiveStockId { get; set; }
        public int LiveStockBreedId { get; set; }
        public int LiveStockCatBreedId { get; set; }
        public int LiveStockStepsId { get; set; }
        public int LiveStockMaterialId { get; set; }
        public int LangCode { get; set; }
        public string VideoURL { get; set; }
        public string LiveStockName { get; set; }
        public string LiveStockBreedName { get; set; }
        public string LiveStockCatBreedName { get; set; }
        public string LiveStockStepsName { get; set; }
        public string LiveStockMaterialName { get; set; }
        public string ImageURL { get; set; }
        public string AudioURL { get; set; }
        public string CreatedOn { get; set; }
        public string notificationContext { get; set; }
        public string category { get; set; }
        public string ResponseMessage { get; set; }
        public string MediaType { get; set; }
        public string FieldType { get; set; }
    }
}