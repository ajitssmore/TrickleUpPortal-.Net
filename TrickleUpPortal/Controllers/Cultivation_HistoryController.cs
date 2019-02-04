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
    public class Cultivation_HistoryController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Cultivation_History
        //public IQueryable<Cultivation_History> GetCultivation_History()
        //{
        //    return db.Cultivation_History;
        //}

        public HttpResponseMessage GetCultivation_History()
        {
            var Cultivation_History = db.Cultivation_History;
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Cultivation_History }, success = true, error = string.Empty });
        }

        public HttpResponseMessage GetCultivation_HistoryByUser(int UserId)
        {
            var Cultivation_History = db.Cultivation_History.Where(a=>a.UserId == UserId);
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Cultivation_History }, success = true, error = string.Empty });
        }

        // GET: api/Cultivation_History/5
        [ResponseType(typeof(Cultivation_History))]
        public IHttpActionResult GetCultivation_History(int id)
        {
            Cultivation_History cultivation_History = db.Cultivation_History.Find(id);
            if (cultivation_History == null)
            {
                return NotFound();
            }

            return Ok(cultivation_History);
        }

        // PUT: api/Cultivation_History/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCultivation_History(int id, Cultivation_History cultivation_History)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cultivation_History.Id)
            {
                return BadRequest();
            }

            db.Entry(cultivation_History).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cultivation_HistoryExists(id))
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

        // POST: api/Cultivation_History
        [ResponseType(typeof(Cultivation_History))]
        public IHttpActionResult PostCultivation_History(Cultivation_History cultivation_History)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cultivation_History.Add(cultivation_History);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cultivation_History.Id }, cultivation_History);
        }

        [HttpPost]
        public HttpResponseMessage PosBulktCultivation_History(Cultivation_History[] cultivation_History)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = "Posted Data is Invalid" });
            }
            foreach (var item in cultivation_History)
            {
                db.Cultivation_History.Add(item);
                db.SaveChanges();
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { cultivation_History }, success = true, error = string.Empty });
        }

        // DELETE: api/Cultivation_History/5
        [ResponseType(typeof(Cultivation_History))]
        public IHttpActionResult DeleteCultivation_History(int id)
        {
            Cultivation_History cultivation_History = db.Cultivation_History.Find(id);
            if (cultivation_History == null)
            {
                return NotFound();
            }

            db.Cultivation_History.Remove(cultivation_History);
            db.SaveChanges();

            return Ok(cultivation_History);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Cultivation_HistoryExists(int id)
        {
            return db.Cultivation_History.Count(e => e.Id == id) > 0;
        }
    }
}