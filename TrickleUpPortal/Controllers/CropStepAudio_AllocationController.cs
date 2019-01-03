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
    public class CropStepAudio_AllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/CropStepAudio_Allocation
        public IQueryable<CropStepAudio_Allocation> GetCropStepAudio_Allocation()
        {
            return db.CropStepAudio_Allocation;
        }

        public HttpResponseMessage GetStep_Audio(int StepId)
        {
            var AudioAllocation = from Audiodata in db.CropStepAudio_Allocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.StepId == StepId && Audiodata.Active == true
                                  select new { Audiodata.Id, Audiodata.StepId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // GET: api/CropStepAudio_Allocation/5
        [ResponseType(typeof(CropStepAudio_Allocation))]
        public IHttpActionResult GetCropStepAudio_Allocation(int id)
        {
            CropStepAudio_Allocation cropStepAudio_Allocation = db.CropStepAudio_Allocation.Find(id);
            if (cropStepAudio_Allocation == null)
            {
                return NotFound();
            }

            return Ok(cropStepAudio_Allocation);
        }

        // PUT: api/CropStepAudio_Allocation/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutCropStepAudio_Allocation(int id, CropStepAudio_Allocation cropStepAudio_Allocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != cropStepAudio_Allocation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(cropStepAudio_Allocation).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CropStepAudio_AllocationExists(id))
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
        public HttpResponseMessage PutCropStepAudio_Allocation(int id, CropStepAudio_Allocation cropStepAudio_Allocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != cropStepAudio_Allocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(cropStepAudio_Allocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropStepAudio_AllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { cropStepAudio_Allocation }, success = true, error = string.Empty });
        }

        // POST: api/CropStepAudio_Allocation
        //[ResponseType(typeof(CropStepAudio_Allocation))]
        //public IHttpActionResult PostCropStepAudio_Allocation(CropStepAudio_Allocation cropStepAudio_Allocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.CropStepAudio_Allocation.Add(cropStepAudio_Allocation);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = cropStepAudio_Allocation.Id }, cropStepAudio_Allocation);
        //}

        [HttpPost]
        public HttpResponseMessage PostCropStepAudio_Allocation(CropStepAudio_Allocation cropStepAudio_Allocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.CropStepAudio_Allocation.Add(cropStepAudio_Allocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = cropStepAudio_Allocation.Id }, success = true, error = string.Empty });
        }


        // DELETE: api/CropStepAudio_Allocation/5
        [ResponseType(typeof(CropStepAudio_Allocation))]
        public IHttpActionResult DeleteCropStepAudio_Allocation(int id)
        {
            CropStepAudio_Allocation cropStepAudio_Allocation = db.CropStepAudio_Allocation.Find(id);
            if (cropStepAudio_Allocation == null)
            {
                return NotFound();
            }

            db.CropStepAudio_Allocation.Remove(cropStepAudio_Allocation);
            db.SaveChanges();

            return Ok(cropStepAudio_Allocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CropStepAudio_AllocationExists(int id)
        {
            return db.CropStepAudio_Allocation.Count(e => e.Id == id) > 0;
        }
    }
}