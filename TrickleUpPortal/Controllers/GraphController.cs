using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrickleUpPortal.Models;
using static TrickleUpPortal.Models.GraphDataModel;

namespace TrickleUpPortal.Controllers
{
    public class GraphController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();


        [HttpGet]
        public HttpResponseMessage GetCropGraphData()
        {
            var query = from psc in db.Cultivation_History
                        join p in db.Crops on psc.CropId equals p.Id
                        where p.Active == true
                        group psc by new { psc.CropId, p.CropName } into g
                        select new
                        {
                            CropName = g.Key.CropName,
                            userCount = g.Select(z => z.UserId).Distinct().Count()
                        };

            //var result = db.Cultivation_History.GroupBy(x => new { x.CropId })
            //         .Select(x => new
            //         {
            //             CropId = x.Key.CropId,
            //             UserCount = x.Select(z => z.UserId).Distinct().Count()
            //         });

            //List<CropGraphData> ListCropData = new List<CropGraphData>();
            //foreach (var item in result)
            //{
            //    string CropName = "";
            //    CropName = db.Crops.Where(a =>a.Id == item.CropId && a.Active==true).Select(a=>a.CropName).FirstOrDefault();
            //    if (!string.IsNullOrEmpty(CropName))
            //    {
            //        CropGraphData objCropData = new CropGraphData();
            //        objCropData.Cropname = CropName;
            //        objCropData.UserCount = item.UserCount;
            //        ListCropData.Add(objCropData);
            //    }
            //}

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = query, success = true, error = string.Empty });
        }
    }
}
