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
    public class CropStep_VideoAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/CropStep_VideoAllocation
        //public IQueryable<CropStep_VideoAllocation> GetCropStep_VideoAllocation()
        //{
        //    return db.CropStep_VideoAllocation;
        //}

        public HttpResponseMessage GetCropStep_VideoAllocation(int StepId)
        {
            var VideoAllocation = from Videodata in db.CropStep_VideoAllocation
                                  join Lang in db.Languages on Videodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Video in db.Videos on Videodata.VideoId equals Video.Id into VideoNew
                                  from Video in VideoNew.DefaultIfEmpty()
                                  where Videodata.StepId == StepId && Videodata.Active == true
                                  select new { Videodata.Id, Videodata.StepId, Videodata.LangId, Lang.LanguageName, Videodata.VideoId, Video.VideoName, Video.FilePath, Videodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { VideoAllocation }, success = true, error = string.Empty });
        }

        // GET: api/CropStep_VideoAllocation/5
        //[ResponseType(typeof(CropStep_VideoAllocation))]
        //public IHttpActionResult GetCropStep_VideoAllocation(int id)
        //{
        //    CropStep_VideoAllocation cropStep_VideoAllocation = db.CropStep_VideoAllocation.Find(id);
        //    if (cropStep_VideoAllocation == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(cropStep_VideoAllocation);
        //}

        // PUT: api/CropStep_VideoAllocation/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutCropStep_VideoAllocation(int id, CropStep_VideoAllocation cropStep_VideoAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != cropStep_VideoAllocation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(cropStep_VideoAllocation).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CropStep_VideoAllocationExists(id))
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
        public HttpResponseMessage PutCropStep_VideoAllocation(int id, CropStep_VideoAllocation cropStep_VideoAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != cropStep_VideoAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(cropStep_VideoAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropStep_VideoAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { cropStep_VideoAllocation }, success = true, error = string.Empty });
        }

        // POST: api/CropStep_VideoAllocation
        //[ResponseType(typeof(CropStep_VideoAllocation))]
        //public IHttpActionResult PostCropStep_VideoAllocation(CropStep_VideoAllocation cropStep_VideoAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.CropStep_VideoAllocation.Add(cropStep_VideoAllocation);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = cropStep_VideoAllocation.Id }, cropStep_VideoAllocation);
        //}
        [HttpPost]
        public HttpResponseMessage PostCropStep_VideoAllocation(CropStep_VideoAllocation cropStep_VideoAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.CropStep_VideoAllocation.Where(a => a.StepId == cropStep_VideoAllocation.StepId && a.LangId == cropStep_VideoAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Video already allocated with this Crop." });
            }

            db.CropStep_VideoAllocation.Add(cropStep_VideoAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = cropStep_VideoAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/CropStep_VideoAllocation/5
        [ResponseType(typeof(CropStep_VideoAllocation))]
        public IHttpActionResult DeleteCropStep_VideoAllocation(int id)
        {
            CropStep_VideoAllocation cropStep_VideoAllocation = db.CropStep_VideoAllocation.Find(id);
            if (cropStep_VideoAllocation == null)
            {
                return NotFound();
            }

            db.CropStep_VideoAllocation.Remove(cropStep_VideoAllocation);
            db.SaveChanges();

            return Ok(cropStep_VideoAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CropStep_VideoAllocationExists(int id)
        {
            return db.CropStep_VideoAllocation.Count(e => e.Id == id) > 0;
        }
    }
}