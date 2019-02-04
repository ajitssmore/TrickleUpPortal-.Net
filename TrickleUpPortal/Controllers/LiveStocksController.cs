using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class LiveStocksController : ApiController
    {
        CommonController comObj = new CommonController();
        string LanguageName;
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStocks
        //public IQueryable<LiveStock> GetLiveStocks()
        //{
        //    return db.LiveStocks;
        //}

        public HttpResponseMessage GetLiveStocks()
        {
            var LiveStock = from LiveStockdata in db.LiveStocks
                            select new { LiveStockdata.Id, LiveStockdata.StockName, LiveStockdata.Active, LiveStockdata.ImageURL};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }

        // GET: api/LiveStocks/5
        [ResponseType(typeof(LiveStock))]
        public IHttpActionResult GetLiveStock(int id)
        {
            LiveStock liveStock = db.LiveStocks.Find(id);
            if (liveStock == null)
            {
                return NotFound();
            }

            return Ok(liveStock);
        }

        // PUT: api/LiveStocks/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutLiveStock(int id, LiveStock liveStock)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != liveStock.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(liveStock).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LiveStockExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}
        [HttpPost]
        public HttpResponseMessage PutLiveStock(int id, LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            //db.Entry(liveStock).State = EntityState.Modified;

            try
            {
                LiveStock LiveStockData = db.LiveStocks.Where(a => a.Id == liveStock.Id).FirstOrDefault();
                LiveStockData.StockName = liveStock.StockName;
                LiveStockData.Active = liveStock.Active;
                if (liveStock.Active == true)
                {
                    LiveStockData.UpdatedBy = liveStock.UpdatedBy;
                    LiveStockData.UpdatedOn = liveStock.UpdatedOn;
                }
                else if (liveStock.Active == false)
                {
                    LiveStockData.ActiveBy = liveStock.ActiveBy;
                    LiveStockData.ActiveOn = liveStock.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStockExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock }, success = true, error = string.Empty });
        }


        // POST: api/LiveStocks
        //[ResponseType(typeof(LiveStock))]
        //public IHttpActionResult PostLiveStock(LiveStock liveStock)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStocks.Add(liveStock);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStock.Id }, liveStock);
        //}

        [HttpPost]
        public HttpResponseMessage PostLiveStock(LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var DataFound = (from LiveStocksdata in db.LiveStocks
                             where LiveStocksdata.StockName.ToUpper() == liveStock.StockName.ToUpper()
                             select LiveStocksdata.StockName).SingleOrDefault();

            if (DataFound == null)
            {
                db.LiveStocks.Add(liveStock);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Stock Name already exists" });
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStocks/5
        [ResponseType(typeof(LiveStock))]
        public IHttpActionResult DeleteLiveStock(int id)
        {
            LiveStock liveStock = db.LiveStocks.Find(id);
            if (liveStock == null)
            {
                return NotFound();
            }

            db.LiveStocks.Remove(liveStock);
            db.SaveChanges();

            return Ok(liveStock);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStockExists(int id)
        {
            return db.LiveStocks.Count(e => e.Id == id) > 0;
        }

        public HttpResponseMessage GetLiveStocksProcessData(int LangCode)
        {
            List<LiveStock> LiveStock = db.LiveStocks.ToList();
            LanguageName = comObj.fetchLang(LangCode);
            if (LiveStock.Count > 0)
            {
                switch (LanguageName)
                {
                    case "Hindi":
                            foreach (LiveStock LiveData in LiveStock)
                            {
                                LiveData.StockName = !string.IsNullOrEmpty(LiveData.StockName) ? comObj.GetResxNameByValue_Hindi(LiveData.StockName) : string.Empty;
                                LiveData.AudioURL = comObj.fetchAudioPahtLiveStock(LiveData.Id, LangCode);
                                Parallel.ForEach(LiveData.LiveStockBreeds, LiveStockBreed =>
                                    {
                                        LiveStockBreed.BreedName = !string.IsNullOrEmpty(LiveStockBreed.BreedName) ? comObj.GetResxNameByValue_Hindi(LiveStockBreed.BreedName) : string.Empty;
                                        Parallel.ForEach(LiveStockBreed.LiveStock_BreedCategory, BreedCategor =>
                                        {
                                            BreedCategor.CategoryName = !string.IsNullOrEmpty(BreedCategor.CategoryName) ? comObj.GetResxNameByValue_Hindi(BreedCategor.CategoryName) : string.Empty;
                                        });
                                    });
                                    
                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                                    {
                                        List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                        List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                        StepData.Adult = AdultData;
                                        StepData.Child = ChildData;
                                    }
                                }
                        break;
                    case "English":
                            foreach (LiveStock LiveData in LiveStock)
                            {
                                foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                                {
                                    List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                    List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                    StepData.Adult = AdultData;
                                    StepData.Child = ChildData;
                                }
                            }
                        break;
                    case "Oriya":
                            foreach (LiveStock LiveData in LiveStock)
                            {
                                LiveData.StockName = !string.IsNullOrEmpty(LiveData.StockName) ? comObj.GetResxNameByValue_Oriya(LiveData.StockName) : string.Empty;
                                Parallel.ForEach(LiveData.LiveStockBreeds, LiveStockBreed =>
                                {
                                    LiveStockBreed.BreedName = !string.IsNullOrEmpty(LiveStockBreed.BreedName) ? comObj.GetResxNameByValue_Oriya(LiveStockBreed.BreedName) : string.Empty;
                                    Parallel.ForEach(LiveStockBreed.LiveStock_BreedCategory, BreedCategor =>
                                    {
                                        BreedCategor.CategoryName = !string.IsNullOrEmpty(BreedCategor.CategoryName) ? comObj.GetResxNameByValue_Oriya(BreedCategor.CategoryName) : string.Empty;
                                    });
                                });
                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                                    {
                                        List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                        List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                        StepData.Adult = AdultData;
                                        StepData.Child = ChildData;
                                    }
                                }
                        break;
                    default:
                        break;
                }
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }
    }
}