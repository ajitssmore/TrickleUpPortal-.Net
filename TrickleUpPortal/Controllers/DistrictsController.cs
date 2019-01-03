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

            var DistrictData = db.Districts.Where(q => q.DistrictName.ToUpper() == district.DistrictName.ToUpper()).Any() ? db.Districts.Where(p => p.DistrictName.ToUpper() == district.DistrictName.ToUpper()).First() : null;
            if (DistrictData != null && DistrictData.Id != district.Id)
            {
                if (db.Districts.Any(p => p.DistrictName.ToUpper() == district.DistrictName.ToUpper()))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "District Name already exists" });
                }
            }
            else
            {
                try
                {
                    District DistrictUpdateData = db.Districts.Where(a => a.Id == district.Id).FirstOrDefault();
                    DistrictUpdateData.DistrictName = district.DistrictName;
                    DistrictUpdateData.State = district.State;
                    DistrictUpdateData.UpdatedBy = district.UpdatedBy;
                    DistrictUpdateData.UpdatedOn = district.UpdatedOn;
                    DistrictUpdateData.Active = district.Active;
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
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { district }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage ActiveDeactiveDistrict(int id, District district)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != district.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            //db.Entry(state).State = EntityState.Modified;

            try
            {
                District DistrictUpdateData = db.Districts.Where(a => a.Id == district.Id).FirstOrDefault();
                DistrictUpdateData.ActiveBy = district.ActiveBy;
                DistrictUpdateData.ActiveOn = district.ActiveOn;
                DistrictUpdateData.Active = district.Active;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(id))
                {
                    //return NotFound();
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

            var DataFound = (from Districtdata in db.Districts
                             where Districtdata.DistrictName.ToUpper() == district.DistrictName.ToUpper()
                                 select Districtdata.DistrictName).SingleOrDefault();
            
            if (DataFound == null)
            {
                db.Districts.Add(district);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "District Name already exists" });
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = district.Id }, success = true, error = string.Empty });
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