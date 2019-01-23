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
    public class CropMaterial_AudioAllocationController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/CropMaterial_AudioAllocation
        //public IQueryable<CropMaterial_AudioAllocation> GetCropMaterial_AudioAllocation()
        //{
        //    return db.CropMaterial_AudioAllocation;
        //}

        public HttpResponseMessage GetCropMaterial_Audio(int MaterialId)
        {
            var AudioAllocation = from Audiodata in db.CropMaterial_AudioAllocation
                                  join Lang in db.Languages on Audiodata.LangId equals Lang.Id into LangNew
                                  from Lang in LangNew.DefaultIfEmpty()
                                  join Audio in db.Audios on Audiodata.AudioId equals Audio.Id into AudioNew
                                  from Audio in AudioNew.DefaultIfEmpty()
                                  where Audiodata.MaterialId == MaterialId
                                  select new { Audiodata.Id, Audiodata.MaterialId, Audiodata.LangId, Lang.LanguageName, Audiodata.FieldType, Audiodata.AudioId, Audio.FileName, Audio.FilePath, Audiodata.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { AudioAllocation }, success = true, error = string.Empty });
        }

        // GET: api/CropMaterial_AudioAllocation/5
        //[ResponseType(typeof(CropMaterial_AudioAllocation))]
        //public IHttpActionResult GetCropMaterial_AudioAllocation(int id)
        //{
        //    CropMaterial_AudioAllocation cropMaterial_AudioAllocation = db.CropMaterial_AudioAllocation.Find(id);
        //    if (cropMaterial_AudioAllocation == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(cropMaterial_AudioAllocation);
        //}

        //// PUT: api/CropMaterial_AudioAllocation/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutCropMaterial_AudioAllocation(int id, CropMaterial_AudioAllocation cropMaterial_AudioAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != cropMaterial_AudioAllocation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(cropMaterial_AudioAllocation).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CropMaterial_AudioAllocationExists(id))
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
        public HttpResponseMessage PutCropMaterial_AudioAllocation(int id, CropMaterial_AudioAllocation cropMaterial_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != cropMaterial_AudioAllocation.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(cropMaterial_AudioAllocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropMaterial_AudioAllocationExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { cropMaterial_AudioAllocation }, success = true, error = string.Empty });
        }

        // POST: api/CropMaterial_AudioAllocation
        //[ResponseType(typeof(CropMaterial_AudioAllocation))]
        //public IHttpActionResult PostCropMaterial_AudioAllocation(CropMaterial_AudioAllocation cropMaterial_AudioAllocation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.CropMaterial_AudioAllocation.Add(cropMaterial_AudioAllocation);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = cropMaterial_AudioAllocation.Id }, cropMaterial_AudioAllocation);
        //}

        [HttpPost]
        public HttpResponseMessage PostCropMaterial_AudioAllocation(CropMaterial_AudioAllocation cropMaterial_AudioAllocation)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            int recordCount = db.CropMaterial_AudioAllocation.Where(a => a.MaterialId == cropMaterial_AudioAllocation.MaterialId && a.LangId == cropMaterial_AudioAllocation.LangId && a.Active == true).Count();
            if (recordCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Audio already allocated with this Crop Step Material." });
            }

            db.CropMaterial_AudioAllocation.Add(cropMaterial_AudioAllocation);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = cropMaterial_AudioAllocation.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/CropMaterial_AudioAllocation/5
        [ResponseType(typeof(CropMaterial_AudioAllocation))]
        public IHttpActionResult DeleteCropMaterial_AudioAllocation(int id)
        {
            CropMaterial_AudioAllocation cropMaterial_AudioAllocation = db.CropMaterial_AudioAllocation.Find(id);
            if (cropMaterial_AudioAllocation == null)
            {
                return NotFound();
            }

            db.CropMaterial_AudioAllocation.Remove(cropMaterial_AudioAllocation);
            db.SaveChanges();

            return Ok(cropMaterial_AudioAllocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CropMaterial_AudioAllocationExists(int id)
        {
            return db.CropMaterial_AudioAllocation.Count(e => e.Id == id) > 0;
        }
    }
}