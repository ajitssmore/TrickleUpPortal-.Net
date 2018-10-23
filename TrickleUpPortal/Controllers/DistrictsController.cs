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
    public class DistrictsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        //// GET: api/Districts
        //public IQueryable<District> GetDistricts()
        //{
        //    return db.Districts;
        //}

        [HttpGet]
        public HttpResponseMessage GetDistricts()
        {
            var Districts = from District in db.Districts
                            join State in db.States on District.State equals State.Id
                select new { District.Id, District.DistrictName, District.State, State.StateName, District.Active};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Districts }, success = true, error = string.Empty });
        }

        // GET: api/Districts/5
        [ResponseType(typeof(District))]
        public IHttpActionResult GetDistrict(int id)
        {
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return NotFound();
            }

            return Ok(district);
        }

        // PUT: api/Districts/5
        //[ResponseType(typeof(void))]
        [HttpPost]
        public HttpResponseMessage PutDistrict(int id, District district)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != district.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(district).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { district }, success = true, error = string.Empty });
        }

        // POST: api/Districts
        [HttpPost]
        public HttpResponseMessage PostDistrict(District district)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Districts.Add(district);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = district.Id }, success = true, error = string.Empty });
            //return CreatedAtRoute("DefaultApi", new { id = district.Id }, district);
        }

        // DELETE: api/Districts/5
        //[ResponseType(typeof(District))]
        //public IHttpActionResult DeleteDistrict(int id)
        //{
        //    District district = db.Districts.Find(id);
        //    if (district == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Districts.Remove(district);
        //    db.SaveChanges();

        //    return Ok(district);
        //}

        [HttpGet]
        public IHttpActionResult DeativeDistrict(int id)
        {
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return NotFound();
            }

            db.Districts.Remove(district);
            db.SaveChanges();

            return Ok(district);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DistrictExists(int id)
        {
            return db.Districts.Count(e => e.Id == id) > 0;
        }
    }
}