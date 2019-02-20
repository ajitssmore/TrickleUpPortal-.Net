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
    public class LiveStock_StepsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStock_Steps
        //public IQueryable<LiveStock_Steps> GetLiveStock_Steps()
        //{
        //    return db.LiveStock_Steps;
        //}

        [HttpGet]
        public HttpResponseMessage GetLiveStock_Steps()
        {
            //return db.LiveStock_Steps;
            var LiveStock_Steps = from LiveStock_Step in db.LiveStock_Steps
                                          join LiveStock in db.LiveStocks on LiveStock_Step.LiveStockId equals LiveStock.Id
                                          select new { LiveStock_Step.Id, LiveStock_Step.StepName, LiveStock_Step.StepDescription, LiveStock_Step.Active, LiveStock_Step.LiveStockId, LiveStock.StockName, LiveStock_Step.StepImageURL };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock_Steps }, success = true, error = string.Empty });
        }

        // GET: api/LiveStock_Steps/5
        [ResponseType(typeof(LiveStock_Steps))]
        public IHttpActionResult GetLiveStock_Steps(int id)
        {
            LiveStock_Steps liveStock_Steps = db.LiveStock_Steps.Find(id);
            if (liveStock_Steps == null)
            {
                return NotFound();
            }

            return Ok(liveStock_Steps);
        }

        // PUT: api/LiveStock_Steps/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutLiveStock_Steps(int id, LiveStock_Steps liveStock_Steps)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != liveStock_Steps.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(liveStock_Steps).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LiveStock_StepsExists(id))
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
        public HttpResponseMessage PutLiveStock_Steps(int id, LiveStock_Steps liveStock_Steps)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock_Steps.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                LiveStock_Steps LiveStock_StepsData = db.LiveStock_Steps.Where(a => a.Id == liveStock_Steps.Id).FirstOrDefault();
                LiveStock_StepsData.StepName = liveStock_Steps.StepName;
                LiveStock_StepsData.StepDescription = liveStock_Steps.StepDescription;
                LiveStock_StepsData.LiveStockId = liveStock_Steps.LiveStockId;
                LiveStock_StepsData.Active = liveStock_Steps.Active;

                if (liveStock_Steps.Active == true)
                {
                    LiveStock_StepsData.UpdatedBy = liveStock_Steps.UpdatedBy;
                    LiveStock_StepsData.UpdatedOn = liveStock_Steps.UpdatedOn;
                }
                else if (liveStock_Steps.Active == false)
                {
                    LiveStock_StepsData.ActiveBy = liveStock_Steps.ActiveBy;
                    LiveStock_StepsData.ActiveOn = liveStock_Steps.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_StepsExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_Steps }, success = true, error = string.Empty });
        }

        // POST: api/LiveStock_Steps
        //[ResponseType(typeof(LiveStock_Steps))]
        //public IHttpActionResult PostLiveStock_Steps(LiveStock_Steps liveStock_Steps)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStock_Steps.Add(liveStock_Steps);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStock_Steps.Id }, liveStock_Steps);
        //}

        [HttpPost]
        public HttpResponseMessage PostLiveStock_Steps(LiveStock_Steps liveStock_Steps)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.LiveStock_Steps.Add(liveStock_Steps);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock_Steps.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStock_Steps/5
        [ResponseType(typeof(LiveStock_Steps))]
        public IHttpActionResult DeleteLiveStock_Steps(int id)
        {
            LiveStock_Steps liveStock_Steps = db.LiveStock_Steps.Find(id);
            if (liveStock_Steps == null)
            {
                return NotFound();
            }

            db.LiveStock_Steps.Remove(liveStock_Steps);
            db.SaveChanges();

            return Ok(liveStock_Steps);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStock_StepsExists(int id)
        {
            return db.LiveStock_Steps.Count(e => e.Id == id) > 0;
        }

        [HttpPost]
        public HttpResponseMessage UpdateLiveStockImagePath(LiveStock_Steps liveStock_Steps)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                LiveStock_Steps LiveStock_StepskData = db.LiveStock_Steps.Where(a => a.Id == liveStock_Steps.Id).FirstOrDefault();
                LiveStock_StepskData.StepImageURL = liveStock_Steps.StepImageURL;

                if (liveStock_Steps.UpdatedBy != null)
                {
                    LiveStock_StepskData.UpdatedBy = liveStock_Steps.UpdatedBy;
                    LiveStock_StepskData.UpdatedOn = liveStock_Steps.UpdatedOn;
                }

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_StepsExists(liveStock_Steps.Id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_Steps }, success = true, error = string.Empty });
        }
    }
}