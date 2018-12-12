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
    public class PredefinedAudiosController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/PredefinedAudios
        //public IQueryable<PredefinedAudio> GetPredefinedAudios()
        //{
        //    return db.PredefinedAudios;
        //}

        [HttpGet]
        public HttpResponseMessage GetPredefinedAudios(int langCode)
        {
            var PredefinedAudios = from PredefinedAudio in db.PredefinedAudios
                                   where PredefinedAudio.LangCode == langCode
                                   select new { PredefinedAudio.Id, PredefinedAudio.PredefinedText, PredefinedAudio.FilePath, PredefinedAudio.LangCode};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { PredefinedAudios }, success = true, error = string.Empty });
        }

        // GET: api/PredefinedAudios/5
        [ResponseType(typeof(PredefinedAudio))]
        public IHttpActionResult GetPredefinedAudio(int id)
        {
            PredefinedAudio predefinedAudio = db.PredefinedAudios.Find(id);
            if (predefinedAudio == null)
            {
                return NotFound();
            }

            return Ok(predefinedAudio);
        }

        // PUT: api/PredefinedAudios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPredefinedAudio(int id, PredefinedAudio predefinedAudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != predefinedAudio.Id)
            {
                return BadRequest();
            }

            db.Entry(predefinedAudio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PredefinedAudioExists(id))
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

        // POST: api/PredefinedAudios
        [ResponseType(typeof(PredefinedAudio))]
        public IHttpActionResult PostPredefinedAudio(PredefinedAudio predefinedAudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PredefinedAudios.Add(predefinedAudio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = predefinedAudio.Id }, predefinedAudio);
        }

        // DELETE: api/PredefinedAudios/5
        [ResponseType(typeof(PredefinedAudio))]
        public IHttpActionResult DeletePredefinedAudio(int id)
        {
            PredefinedAudio predefinedAudio = db.PredefinedAudios.Find(id);
            if (predefinedAudio == null)
            {
                return NotFound();
            }

            db.PredefinedAudios.Remove(predefinedAudio);
            db.SaveChanges();

            return Ok(predefinedAudio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PredefinedAudioExists(int id)
        {
            return db.PredefinedAudios.Count(e => e.Id == id) > 0;
        }
    }
}