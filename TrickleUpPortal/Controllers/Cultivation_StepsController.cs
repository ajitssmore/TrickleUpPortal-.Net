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
    public class Cultivation_StepsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Cultivation_Steps
        //public IQueryable<Cultivation_Steps> GetCultivation_Steps()
        //{
        //    return db.Cultivation_Steps;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetCultivation_Steps(int langCode)
        //{
        //    var Cultivation_Steps = from Cultivation_Step in db.Cultivation_Steps
        //                            join crops in db.Crops on Cultivation_Step.Crop_Id equals crops.Id
        //                            select new { Cultivation_Step.Id, Cultivation_Step.Crop_Id, crops.CropName, Cultivation_Step.Step_Name, Cultivation_Step.Step_Description, Cultivation_Step.MediaURL };
        //    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Cultivation_Steps }, success = true, error = string.Empty });
        //}

        [HttpGet]
        public HttpResponseMessage GetCultivation_StepsLan(int langCode)
        {
            var Cultivation_Steps = from Cultivation_Step in db.Cultivation_Steps
                                    join crops in db.Crops on Cultivation_Step.Crop_Id equals crops.Id
                                    select new { Cultivation_Step.Id, Cultivation_Step.Crop_Id, crops.CropName, Cultivation_Step.Step_Name, Cultivation_Step.Step_Description, Cultivation_Step.MediaURL };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Cultivation_Steps }, success = true, error = string.Empty });
        }

        // GET: api/Cultivation_Steps/5
        [ResponseType(typeof(Cultivation_Steps))]
        public IHttpActionResult GetCultivation_Steps(int id)
        {
            Cultivation_Steps cultivation_Steps = db.Cultivation_Steps.Find(id);
            if (cultivation_Steps == null)
            {
                return NotFound();
            }

            return Ok(cultivation_Steps);
        }

        // PUT: api/Cultivation_Steps/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCultivation_Steps(int id, Cultivation_Steps cultivation_Steps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cultivation_Steps.Id)
            {
                return BadRequest();
            }

            db.Entry(cultivation_Steps).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cultivation_StepsExists(id))
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

        // POST: api/Cultivation_Steps
        [ResponseType(typeof(Cultivation_Steps))]
        public IHttpActionResult PostCultivation_Steps(Cultivation_Steps cultivation_Steps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cultivation_Steps.Add(cultivation_Steps);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cultivation_Steps.Id }, cultivation_Steps);
        }

        // DELETE: api/Cultivation_Steps/5
        [ResponseType(typeof(Cultivation_Steps))]
        public IHttpActionResult DeleteCultivation_Steps(int id)
        {
            Cultivation_Steps cultivation_Steps = db.Cultivation_Steps.Find(id);
            if (cultivation_Steps == null)
            {
                return NotFound();
            }

            db.Cultivation_Steps.Remove(cultivation_Steps);
            db.SaveChanges();

            return Ok(cultivation_Steps);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Cultivation_StepsExists(int id)
        {
            return db.Cultivation_Steps.Count(e => e.Id == id) > 0;
        }
    }
}