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
    public class LiveStock_Steps_AudioAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStock_Steps_AudioAllocation
        public IQueryable<LiveStock_Steps_AudioAllocation> GetLiveStock_Steps_AudioAllocation()
        {
            return db.LiveStock_Steps_AudioAllocation;
        }

        // GET: api/LiveStock_Steps_AudioAllocation/5
        [HttpGet]
        public HttpResponseMessage GetLiveStock_Steps_AudioAllocation(int id)
        {
            var AudioAllocation = from Audiodata in db.LiveStock_Steps_AudioAllocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.LiveStockStepId == id && Audiodata.Active == true
                                  select new { Audiodata.Id, Audiodata.LiveStockStepId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // PUT: api/LiveStock_Steps_AudioAllocation/5
        [HttpPost]
        public HttpResponseMessage PutLiveStock_Steps_AudioAllocation(int id, LiveStock_Steps_AudioAllocation liveStock_Steps_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock_Steps_AudioAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(liveStock_Steps_AudioAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_Steps_AudioAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_Steps_AudioAllocation }, success = true, error = string.Empty });
        }

        // POST: api/LiveStock_Steps_AudioAllocation
        [HttpPost]
        public HttpResponseMessage PostLiveStock_Steps_AudioAllocation(LiveStock_Steps_AudioAllocation liveStock_Steps_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.LiveStock_Steps_AudioAllocation.Where(a => a.LiveStockStepId == liveStock_Steps_AudioAllocation.LiveStockStepId && a.LangId == liveStock_Steps_AudioAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Audio already allocated with this Live Stock Breed Category." });
            }

            db.LiveStock_Steps_AudioAllocation.Add(liveStock_Steps_AudioAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock_Steps_AudioAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStock_Steps_AudioAllocation/5
        [ResponseType(typeof(LiveStock_Steps_AudioAllocation))]
        public IHttpActionResult DeleteLiveStock_Steps_AudioAllocation(int id)
        {
            LiveStock_Steps_AudioAllocation liveStock_Steps_AudioAllocation = db.LiveStock_Steps_AudioAllocation.Find(id);
            if (liveStock_Steps_AudioAllocation == null)
            {
                return NotFound();
            }

            db.LiveStock_Steps_AudioAllocation.Remove(liveStock_Steps_AudioAllocation);
            db.SaveChanges();

            return Ok(liveStock_Steps_AudioAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStock_Steps_AudioAllocationExists(int id)
        {
            return db.LiveStock_Steps_AudioAllocation.Count(e => e.Id == id) > 0;
        }
    }
}