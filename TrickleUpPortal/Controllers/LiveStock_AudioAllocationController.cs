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
    public class LiveStock_AudioAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStock_AudioAllocation
        //public IQueryable<LiveStock_AudioAllocation> GetLiveStock_AudioAllocation()
        //{
        //    return db.LiveStock_AudioAllocation;
        //}

        public HttpResponseMessage GetLiveStock_AudioAllocation(int LiveStockId)
        {
            var AudioAllocation = from Audiodata in db.LiveStock_AudioAllocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.LiveStockId == LiveStockId && Audiodata.Active == true
                                  select new { Audiodata.Id, Audiodata.LiveStockId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // GET: api/LiveStock_AudioAllocation/5
        //[ResponseType(typeof(LiveStock_AudioAllocation))]
        //public IHttpActionResult GetLiveStock_AudioAllocation(int id)
        //{
        //    LiveStock_AudioAllocation liveStock_AudioAllocation = db.LiveStock_AudioAllocation.Find(id);
        //    if (liveStock_AudioAllocation == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(liveStock_AudioAllocation);
        //}

        // PUT: api/LiveStock_AudioAllocation/5
        [HttpPost]
        public HttpResponseMessage PutLiveStock_AudioAllocation(int id, LiveStock_AudioAllocation liveStock_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock_AudioAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(liveStock_AudioAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_AudioAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_AudioAllocation }, success = true, error = string.Empty });
        }

        // POST: api/LiveStock_AudioAllocation
        //[ResponseType(typeof(LiveStock_AudioAllocation))]
        //public IHttpActionResult PostLiveStock_AudioAllocation(LiveStock_AudioAllocation liveStock_AudioAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStock_AudioAllocation.Add(liveStock_AudioAllocation);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStock_AudioAllocation.Id }, liveStock_AudioAllocation);
        //}

        [HttpPost]
        public HttpResponseMessage PostLiveStock_AudioAllocation(LiveStock_AudioAllocation liveStock_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.LiveStock_AudioAllocation.Where(a => a.LiveStockId == liveStock_AudioAllocation.LiveStockId && a.LangId == liveStock_AudioAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Audio already allocated with this Live Stock." });
            }

            db.LiveStock_AudioAllocation.Add(liveStock_AudioAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock_AudioAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStock_AudioAllocation/5
        [ResponseType(typeof(LiveStock_AudioAllocation))]
        public IHttpActionResult DeleteLiveStock_AudioAllocation(int id)
        {
            LiveStock_AudioAllocation liveStock_AudioAllocation = db.LiveStock_AudioAllocation.Find(id);
            if (liveStock_AudioAllocation == null)
            {
                return NotFound();
            }

            db.LiveStock_AudioAllocation.Remove(liveStock_AudioAllocation);
            db.SaveChanges();

            return Ok(liveStock_AudioAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStock_AudioAllocationExists(int id)
        {
            return db.LiveStock_AudioAllocation.Count(e => e.Id == id) > 0;
        }
    }
}