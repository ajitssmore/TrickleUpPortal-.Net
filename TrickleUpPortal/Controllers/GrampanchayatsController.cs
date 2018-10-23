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
    public class GrampanchayatsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Grampanchayats
        //public IQueryable<Grampanchayat> GetGrampanchayats()
        //{
        //    return db.Grampanchayats;
        //}
        [HttpGet]
        public HttpResponseMessage GetGrampanchayats()
        {
            var Grampanchayatdata = from Grampanchayat in db.Grampanchayats
                                    join District in db.Districts on Grampanchayat.District equals District.Id
                                    join State in db.States on Grampanchayat.State equals State.Id
                                    select new { Grampanchayat.Id, Grampanchayat.GrampanchayatName, Grampanchayat.State, State.StateName, Grampanchayat.District, District.DistrictName, Grampanchayat.Active};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Grampanchayatdata }, success = true, error = string.Empty });
        }

        // GET: api/Grampanchayats/5
        [ResponseType(typeof(Grampanchayat))]
        public IHttpActionResult GetGrampanchayat(int id)
        {
            Grampanchayat grampanchayat = db.Grampanchayats.Find(id);
            if (grampanchayat == null)
            {
                return NotFound();
            }

            return Ok(grampanchayat);
        }

        // PUT: api/Grampanchayats/5
        //[ResponseType(typeof(void))]
        [HttpPost]
        public HttpResponseMessage PutGrampanchayat(int id, Grampanchayat grampanchayat)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != grampanchayat.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(grampanchayat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrampanchayatExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { grampanchayat }, success = true, error = string.Empty });
        }

        // POST: api/Grampanchayats
        //[ResponseType(typeof(Grampanchayat))]
        [HttpPost]
        public HttpResponseMessage PostGrampanchayat(Grampanchayat grampanchayat)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Grampanchayats.Add(grampanchayat);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = grampanchayat.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Grampanchayats/5
        [ResponseType(typeof(Grampanchayat))]
        public IHttpActionResult DeleteGrampanchayat(int id)
        {
            Grampanchayat grampanchayat = db.Grampanchayats.Find(id);
            if (grampanchayat == null)
            {
                return NotFound();
            }

            db.Grampanchayats.Remove(grampanchayat);
            db.SaveChanges();

            return Ok(grampanchayat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrampanchayatExists(int id)
        {
            return db.Grampanchayats.Count(e => e.Id == id) > 0;
        }
    }
}