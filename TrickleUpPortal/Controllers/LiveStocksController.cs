using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class LiveStocksController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStocks
        //public IQueryable<LiveStock> GetLiveStocks()
        //{
        //    return db.LiveStocks;
        //}

        public HttpResponseMessage GetLiveStocks(int LangCode)
        {
            var LiveStock = from LiveStockdata in db.LiveStocks
                            where LiveStockdata.Active == true
                            select new { LiveStockdata.Id, LiveStockdata.StockName, LiveStockdata.ImageURL, LiveStockdata.AudioURL, LiveStockdata.Active };
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLiveStock(int id, LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != liveStock.Id)
            {
                return BadRequest();
            }

            db.Entry(liveStock).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LiveStocks
        [ResponseType(typeof(LiveStock))]
        public IHttpActionResult PostLiveStock(LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LiveStocks.Add(liveStock);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = liveStock.Id }, liveStock);
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
            if (LiveStock.Count > 0)
            {
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
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }

        public HttpResponseMessage GetLiveStocksProcessDataNew(int LangCode)
        {
            List<LiveStock> LiveStock = db.LiveStocks.ToList();
            //dynamic LiveStockProcessData = new ExpandoObject();
            //if (LiveStock.Count > 0)
            //{
            //    LiveStockProcessData = LiveStock;
            //    int LiveStockIndex = 0, LiveStock_StepsIndex = 0;
            //    foreach (var LiveData in LiveStockProcessData)
            //    {
            //        foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
            //        {
            //            List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
            //            List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
            //            LiveStockProcessData[LiveStockIndex].LiveStock_Steps[LiveStock_StepsIndex].Add(new[] { "toto" }) ; //= new[] { AdultData };
            //            LiveStockProcessData[LiveStockIndex].LiveStock_Steps[LiveStock_StepsIndex].ChildData = ChildData;
            //            LiveStock_StepsIndex++;
            //        }
            //        LiveStockIndex++;
            //    }
            //}

            if (LiveStock.Count > 0)
            {
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
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }
    }
}