using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;
using System.Resources;
using System.Collections;

namespace TrickleUpPortal.Controllers
{
    public class CropsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        public object serializer { get; private set; }

        //// GET: api/Crops
        //public IQueryable<Crop> GetCrops()
        //{
        //    return db.Crops;
        //}

        [HttpGet]
        public HttpResponseMessage GetCrops()
        {
            string lang = "Hindi";
            var results = from Cropdata in db.Crops
                          select new { Cropdata.Id, Cropdata.CropName, Cropdata.FilePath, Cropdata.Ready };
            List<Cropdata> Crops = new List<Cropdata>();
            foreach (var item in results)
            {
                if (lang == "Hindi")
                {
                    Cropdata cropObj = new Cropdata();
                    cropObj.Id = item.Id;
                    cropObj.CropName = GetResxNameByValue(item.CropName);
                    cropObj.FilePath = item.FilePath;
                    cropObj.Ready = (bool)item.Ready;
                    Crops.Add(cropObj);
                }
                else
                {
                    Cropdata cropObj = new Cropdata();
                    cropObj.Id = item.Id;
                    cropObj.CropName = item.CropName;
                    cropObj.FilePath = item.FilePath;
                    cropObj.Ready = (bool)item.Ready;
                    Crops.Add(cropObj);
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Crops }, success = true, error = string.Empty });
        }

        private string GetResxNameByValue(string value)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("TrickleUpPortal.Resources.Lang_hindi", this.GetType().Assembly);
            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Key.ToString() == value.Replace(" ", "").ToString());

            var key = entry.Value.ToString();
            return key;

        }

        public class Cropdata
        {
            public int Id { get; set; }
            public string CropName { get; set; }
            public string FilePath { get; set; }
            public bool Ready { get; set; }
        }

        //// GET: api/Crops/5
        //[ResponseType(typeof(Crop))]
        //public IHttpActionResult GetCrop(int id)
        //{
        //    Crop crop = db.Crops.Find(id);
        //    if (crop == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(crop);
        //}

        [HttpGet]
        public HttpResponseMessage GetCropStepMaterialData(int id)
        {
            try
            {
                Crop crop = db.Crops.Find(id);

                if (crop == null)
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty, success = false, error = "No Data" });
                }
                else
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = crop, success = true, error = (string)null });
                }
            }
            catch (Exception ex)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = (string)null, success = true, error = ex.Message });
            }
        }

        // PUT: api/Crops/5
        [HttpPost]
        public HttpResponseMessage PutCrop(int id, Crop crop)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != crop.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(crop).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { crop }, success = true, error = string.Empty });
        }

        // POST: api/Crops
        [HttpPost]
        public HttpResponseMessage PostCrop(Crop crop)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Crops.Add(crop);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = crop.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Crops/5
        //[ResponseType(typeof(Crop))]
        //public IHttpActionResult DeleteCrop(int id)
        //{
        //    Crop crop = db.Crops.Find(id);
        //    if (crop == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Crops.Remove(crop);
        //    db.SaveChanges();

        //    return Ok(crop);
        //}

        [HttpGet]
        public IHttpActionResult DeativeCrop(int id)
        {
            Crop crop = db.Crops.Find(id);
            if (crop == null)
            {
                return NotFound();
            }

            db.Crops.Remove(crop);
            db.SaveChanges();

            return Ok(crop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CropExists(int id)
        {
            return db.Crops.Count(e => e.Id == id) > 0;
        }
    }
}