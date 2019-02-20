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
    public class LiveStock_BreedCategory_AudioAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStock_BreedCategory_AudioAllocation
        public IQueryable<LiveStock_BreedCategory_AudioAllocation> GetLiveStock_BreedCategory_AudioAllocation()
        {
            return db.LiveStock_BreedCategory_AudioAllocation;
        }


        // GET: api/LiveStock_BreedCategory_AudioAllocation/5
        [HttpGet]
        public HttpResponseMessage GetLiveStock_BreedCategory_AudioAllocation(int id)
        {
            var AudioAllocation = from Audiodata in db.LiveStock_BreedCategory_AudioAllocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.LiveStockBreedCategoryId == id && Audiodata.Active == true
                                  select new { Audiodata.Id, Audiodata.LiveStockBreedCategoryId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // PUT: api/LiveStock_BreedCategory_AudioAllocation/5
        [HttpPost]
        public HttpResponseMessage PutLiveStock_BreedCategory_AudioAllocation(int id, LiveStock_BreedCategory_AudioAllocation liveStock_BreedCategory_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock_BreedCategory_AudioAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(liveStock_BreedCategory_AudioAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_BreedCategory_AudioAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_BreedCategory_AudioAllocation }, success = true, error = string.Empty });
        }

        // POST: api/LiveStock_BreedCategory_AudioAllocation
        [HttpPost]
        public HttpResponseMessage PostLiveStock_BreedCategory_AudioAllocation(LiveStock_BreedCategory_AudioAllocation liveStock_BreedCategory_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.LiveStock_BreedCategory_AudioAllocation.Where(a => a.LiveStockBreedCategoryId == liveStock_BreedCategory_AudioAllocation.LiveStockBreedCategoryId && a.LangId == liveStock_BreedCategory_AudioAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Audio already allocated with this Live Stock Breed Category." });
            }

            db.LiveStock_BreedCategory_AudioAllocation.Add(liveStock_BreedCategory_AudioAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock_BreedCategory_AudioAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStock_BreedCategory_AudioAllocation/5
        [ResponseType(typeof(LiveStock_BreedCategory_AudioAllocation))]
        public IHttpActionResult DeleteLiveStock_BreedCategory_AudioAllocation(int id)
        {
            LiveStock_BreedCategory_AudioAllocation liveStock_BreedCategory_AudioAllocation = db.LiveStock_BreedCategory_AudioAllocation.Find(id);
            if (liveStock_BreedCategory_AudioAllocation == null)
            {
                return NotFound();
            }

            db.LiveStock_BreedCategory_AudioAllocation.Remove(liveStock_BreedCategory_AudioAllocation);
            db.SaveChanges();

            return Ok(liveStock_BreedCategory_AudioAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStock_BreedCategory_AudioAllocationExists(int id)
        {
            return db.LiveStock_BreedCategory_AudioAllocation.Count(e => e.Id == id) > 0;
        }
    }
}