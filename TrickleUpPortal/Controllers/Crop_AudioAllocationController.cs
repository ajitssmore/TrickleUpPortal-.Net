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
    public class Crop_AudioAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Crop_AudioAllocation
        //public IQueryable<Crop_AudioAllocation> GetCrop_AudioAllocation()
        //{
        //    return db.Crop_AudioAllocation;
        //}

        public HttpResponseMessage Get_CropAudioAllocation(int CropId)
        {
            var AudioAllocation = from Audiodata in db.Crop_AudioAllocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.CropId == CropId && Audiodata.Active == true
                                  select new { Audiodata.Id, Audiodata.CropId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // GET: api/Crop_AudioAllocation/5
        [ResponseType(typeof(Crop_AudioAllocation))]
        public IHttpActionResult GetCrop_AudioAllocation(int id)
        {
            Crop_AudioAllocation crop_AudioAllocation = db.Crop_AudioAllocation.Find(id);
            if (crop_AudioAllocation == null)
            {
                return NotFound();
            }

            return Ok(crop_AudioAllocation);
        }

        // PUT: api/Crop_AudioAllocation/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutCrop_AudioAllocation(int id, Crop_AudioAllocation crop_AudioAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != crop_AudioAllocation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(crop_AudioAllocation).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Crop_AudioAllocationExists(id))
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
        public HttpResponseMessage PutCrop_AudioAllocation(int id, Crop_AudioAllocation crop_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != crop_AudioAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(crop_AudioAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Crop_AudioAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { crop_AudioAllocation }, success = true, error = string.Empty });
        }


        // POST: api/Crop_AudioAllocation
        //[ResponseType(typeof(Crop_AudioAllocation))]
        //public IHttpActionResult PostCrop_AudioAllocation(Crop_AudioAllocation crop_AudioAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Crop_AudioAllocation.Add(crop_AudioAllocation);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = crop_AudioAllocation.Id }, crop_AudioAllocation);
        //}

        [HttpPost]
        public HttpResponseMessage PostCrop_AudioAllocation(Crop_AudioAllocation crop_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.Crop_AudioAllocation.Where(a => a.CropId == crop_AudioAllocation.CropId && a.LangId == crop_AudioAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Audio already allocated with this Crop." });
            }

            db.Crop_AudioAllocation.Add(crop_AudioAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = crop_AudioAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Crop_AudioAllocation/5
        [ResponseType(typeof(Crop_AudioAllocation))]
        public IHttpActionResult DeleteCrop_AudioAllocation(int id)
        {
            Crop_AudioAllocation crop_AudioAllocation = db.Crop_AudioAllocation.Find(id);
            if (crop_AudioAllocation == null)
            {
                return NotFound();
            }

            db.Crop_AudioAllocation.Remove(crop_AudioAllocation);
            db.SaveChanges();

            return Ok(crop_AudioAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Crop_AudioAllocationExists(int id)
        {
            return db.Crop_AudioAllocation.Count(e => e.Id == id) > 0;
        }
    }
}