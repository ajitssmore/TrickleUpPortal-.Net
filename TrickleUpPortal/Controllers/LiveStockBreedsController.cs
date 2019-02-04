using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class LiveStockBreedsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStockBreeds
        //public IQueryable<LiveStockBreed> GetLiveStockBreeds()
        //{
        //    return db.LiveStockBreeds;
        //}
        [HttpGet]
        public HttpResponseMessage GetLiveStockBreeds()
        {
            var LiveStockBreeds = from LiveStockBreed in db.LiveStockBreeds
                                  join LiveStock in db.LiveStocks on LiveStockBreed.LiveStockId equals LiveStock.Id
                                  select new { LiveStockBreed.Id, LiveStockBreed.BreedName, LiveStockBreed.LiveStockId, LiveStock.StockName, LiveStockBreed.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStockBreeds }, success = true, error = string.Empty });

            //return db.LiveStockBreeds;
        }

        // GET: api/LiveStockBreeds/5
        [ResponseType(typeof(LiveStockBreed))]
        public IHttpActionResult GetLiveStockBreed(int id)
        {
            LiveStockBreed liveStockBreed = db.LiveStockBreeds.Find(id);
            if (liveStockBreed == null)
            {
                return NotFound();
            }

            return Ok(liveStockBreed);
        }

        // PUT: api/LiveStockBreeds/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutLiveStockBreed(int id, LiveStockBreed liveStockBreed)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != liveStockBreed.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(liveStockBreed).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LiveStockBreedExists(id))
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
        public HttpResponseMessage PutLiveStockBreed(int id, LiveStockBreed liveStockBreed)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStockBreed.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                LiveStockBreed LiveStockBreedData = db.LiveStockBreeds.Where(a => a.Id == liveStockBreed.Id).FirstOrDefault();
                LiveStockBreedData.BreedName = liveStockBreed.BreedName;
                LiveStockBreedData.LiveStockId = liveStockBreed.LiveStockId;
                LiveStockBreedData.Active = liveStockBreed.Active;

                if (liveStockBreed.Active == true)
                {
                    LiveStockBreedData.UpdatedBy = liveStockBreed.UpdatedBy;
                    LiveStockBreedData.UpdatedOn = liveStockBreed.UpdatedOn;
                }
                else if (liveStockBreed.Active == false)
                {
                    LiveStockBreedData.ActiveBy = liveStockBreed.ActiveBy;
                    LiveStockBreedData.ActiveOn = liveStockBreed.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStockBreedExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStockBreed }, success = true, error = string.Empty });
        }

        // POST: api/LiveStockBreeds
        //[ResponseType(typeof(LiveStockBreed))]
        //public IHttpActionResult PostLiveStockBreed(LiveStockBreed liveStockBreed)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStockBreeds.Add(liveStockBreed);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStockBreed.Id }, liveStockBreed);
        //}

        [HttpPost]
        public HttpResponseMessage PostLiveStockBreed(LiveStockBreed liveStockBreed)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.LiveStockBreeds.Add(liveStockBreed);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStockBreed.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStockBreeds/5
        [ResponseType(typeof(LiveStockBreed))]
        public IHttpActionResult DeleteLiveStockBreed(int id)
        {
            LiveStockBreed liveStockBreed = db.LiveStockBreeds.Find(id);
            if (liveStockBreed == null)
            {
                return NotFound();
            }

            db.LiveStockBreeds.Remove(liveStockBreed);
            db.SaveChanges();

            return Ok(liveStockBreed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStockBreedExists(int id)
        {
            return db.LiveStockBreeds.Count(e => e.Id == id) > 0;
        }
    }
}