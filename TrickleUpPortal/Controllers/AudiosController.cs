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
    public class AudiosController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Audios
        //public IQueryable<Audio> GetAudios()
        //{
        //    return db.Audios;
        //}

        public HttpResponseMessage GetAudios()
        {
            var Audios = from Audio in db.Audios
                         select new { Audio.Id, Audio.FileName, Audio.FilePath, Audio.FileSize, Audio.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Audios }, success = true, error = string.Empty });
        }

        // GET: api/Audios/5
        [ResponseType(typeof(Audio))]
        public IHttpActionResult GetAudio(int id)
        {
            Audio audio = db.Audios.Find(id);
            if (audio == null)
            {
                return NotFound();
            }

            return Ok(audio);
        }

        // PUT: api/Audios/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutAudio(int id, Audio audio)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != audio.Id)
        //    {
        //        return BadRequest();
        //    }

        //    //db.Entry(audio).State = EntityState.Modified;

        //    try
        //    {
        //        Audio audiodata = db.Audios.Where(a => a.Id == audio.Id).FirstOrDefault();
        //        audiodata.Active = audio.Active;
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AudioExists(id))
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
        public HttpResponseMessage DeactiveAudio(int id, Audio audio)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != audio.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            //db.Entry(audio).State = EntityState.Modified;

            try
            {
                Audio audiodata = db.Audios.Where(a => a.Id == audio.Id).FirstOrDefault();
                audiodata.Active = audio.Active;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AudioExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { audio }, success = true, error = string.Empty });
        }

        // POST: api/Audios
        [ResponseType(typeof(Audio))]
        public HttpResponseMessage PostAudio(Audio audio)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Audios.Add(audio);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = audio.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Audios/5
        [ResponseType(typeof(Audio))]
        public IHttpActionResult DeleteAudio(int id)
        {
            Audio audio = db.Audios.Find(id);
            if (audio == null)
            {
                return NotFound();
            }

            db.Audios.Remove(audio);
            db.SaveChanges();

            return Ok(audio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AudioExists(int id)
        {
            return db.Audios.Count(e => e.Id == id) > 0;
        }
    }
}