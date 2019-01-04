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
    public class Crop_VideoAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Crop_VideoAllocation
        //public IQueryable<Crop_VideoAllocation> GetCrop_VideoAllocation()
        //{
        //    return db.Crop_VideoAllocation;
        //}

        public HttpResponseMessage GetCrop_VideoAllocation(int CropId)
        {
            var VideoAllocation = from Videodata in db.Crop_VideoAllocation
                                  join Lang in db.Languages on Videodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Video in db.Videos on Videodata.VideoId equals Video.Id into VideoNew
                                  from Video in VideoNew.DefaultIfEmpty()
                                  where Videodata.CropId == CropId && Videodata.Active == true
                                  select new { Videodata.Id, Videodata.CropId, Videodata.LangId, Lang.LanguageName, Videodata.VideoId, Video.VideoName, Video.FilePath, Videodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { VideoAllocation }, success = true, error = string.Empty });
        }

        // GET: api/Crop_VideoAllocation/5
        //[ResponseType(typeof(Crop_VideoAllocation))]
        //public IHttpActionResult GetCrop_VideoAllocation(int id)
        //{
        //    Crop_VideoAllocation crop_VideoAllocation = db.Crop_VideoAllocation.Find(id);
        //    if (crop_VideoAllocation == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(crop_VideoAllocation);
        //}

        // PUT: api/Crop_VideoAllocation/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCrop_VideoAllocation(int id, Crop_VideoAllocation crop_VideoAllocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != crop_VideoAllocation.Id)
            {
                return BadRequest();
            }

            db.Entry(crop_VideoAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Crop_VideoAllocationExists(id))
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

        // POST: api/Crop_VideoAllocation
        [ResponseType(typeof(Crop_VideoAllocation))]
        public IHttpActionResult PostCrop_VideoAllocation(Crop_VideoAllocation crop_VideoAllocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Crop_VideoAllocation.Add(crop_VideoAllocation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = crop_VideoAllocation.Id }, crop_VideoAllocation);
        }

        // DELETE: api/Crop_VideoAllocation/5
        [ResponseType(typeof(Crop_VideoAllocation))]
        public IHttpActionResult DeleteCrop_VideoAllocation(int id)
        {
            Crop_VideoAllocation crop_VideoAllocation = db.Crop_VideoAllocation.Find(id);
            if (crop_VideoAllocation == null)
            {
                return NotFound();
            }

            db.Crop_VideoAllocation.Remove(crop_VideoAllocation);
            db.SaveChanges();

            return Ok(crop_VideoAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Crop_VideoAllocationExists(int id)
        {
            return db.Crop_VideoAllocation.Count(e => e.Id == id) > 0;
        }
    }
}