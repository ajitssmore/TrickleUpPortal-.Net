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
    public class LiveStockBreed_AudioAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStockBreed_AudioAllocation
        //public IQueryable<LiveStockBreed_AudioAllocation> GetLiveStockBreed_AudioAllocation()
        //{
        //    return db.LiveStockBreed_AudioAllocation;
        //}

        public HttpResponseMessage GetLiveStockBreed_AudioAllocation(int LiveStockBreedId)
        {
            var AudioAllocation = from Audiodata in db.LiveStockBreed_AudioAllocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.LiveStockBreedId == LiveStockBreedId && Audiodata.Active == true
                                  select new { Audiodata.Id, Audiodata.LiveStockBreedId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // GET: api/LiveStockBreed_AudioAllocation/5
        //[ResponseType(typeof(LiveStockBreed_AudioAllocation))]
        //public IHttpActionResult GetLiveStockBreed_AudioAllocation(int id)
        //{
        //    LiveStockBreed_AudioAllocation liveStockBreed_AudioAllocation = db.LiveStockBreed_AudioAllocation.Find(id);
        //    if (liveStockBreed_AudioAllocation == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(liveStockBreed_AudioAllocation);
        //}

        // PUT: api/LiveStockBreed_AudioAllocation/5
        [HttpPost]
        public HttpResponseMessage PutLiveStockBreed_AudioAllocation(int id, LiveStockBreed_AudioAllocation liveStockBreed_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStockBreed_AudioAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(liveStockBreed_AudioAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStockBreed_AudioAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStockBreed_AudioAllocation }, success = true, error = string.Empty });
        }

        // POST: api/LiveStockBreed_AudioAllocation
        //[ResponseType(typeof(LiveStockBreed_AudioAllocation))]
        //public IHttpActionResult PostLiveStockBreed_AudioAllocation(LiveStockBreed_AudioAllocation liveStockBreed_AudioAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStockBreed_AudioAllocation.Add(liveStockBreed_AudioAllocation);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStockBreed_AudioAllocation.Id }, liveStockBreed_AudioAllocation);
        //}
        [HttpPost]
        public HttpResponseMessage PostLiveStockBreed_AudioAllocation(LiveStockBreed_AudioAllocation liveStockBreed_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.LiveStockBreed_AudioAllocation.Where(a => a.LiveStockBreedId == liveStockBreed_AudioAllocation.LiveStockBreedId && a.LangId == liveStockBreed_AudioAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Audio already allocated with this Live Stock Breed." });
            }

            db.LiveStockBreed_AudioAllocation.Add(liveStockBreed_AudioAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStockBreed_AudioAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStockBreed_AudioAllocation/5
        [ResponseType(typeof(LiveStockBreed_AudioAllocation))]
        public IHttpActionResult DeleteLiveStockBreed_AudioAllocation(int id)
        {
            LiveStockBreed_AudioAllocation liveStockBreed_AudioAllocation = db.LiveStockBreed_AudioAllocation.Find(id);
            if (liveStockBreed_AudioAllocation == null)
            {
                return NotFound();
            }

            db.LiveStockBreed_AudioAllocation.Remove(liveStockBreed_AudioAllocation);
            db.SaveChanges();

            return Ok(liveStockBreed_AudioAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStockBreed_AudioAllocationExists(int id)
        {
            return db.LiveStockBreed_AudioAllocation.Count(e => e.Id == id) > 0;
        }
    }
}