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
    public class GendersController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Genders
        //public IQueryable<Gender> GetGenders()
        //{
        //    return db.Genders;
        //}

        [HttpGet]
        public HttpResponseMessage GetGenders()
        {
            var Genders = from Gender in db.Genders
                         select new { Gender.Id, Gender.GenderName};
            //return Ok(result);
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Genders }, success = true, error = string.Empty });
        }

        // GET: api/Genders/5
        [ResponseType(typeof(Gender))]
        public IHttpActionResult GetGender(int id)
        {
            Gender gender = db.Genders.Find(id);
            if (gender == null)
            {
                return NotFound();
            }

            return Ok(gender);
        }

        // PUT: api/Genders/5
        [HttpPost]
        public HttpResponseMessage PutGender(int id, Gender gender)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != gender.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(gender).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenderExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { gender }, success = true, error = string.Empty });
        }

        // POST: api/Genders
        [HttpPost]
        public HttpResponseMessage PostGender(Gender gender)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Genders.Add(gender);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = gender.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Genders/5
        //[ResponseType(typeof(Gender))]
        //public IHttpActionResult DeleteGender(int id)
        //{
        //    Gender gender = db.Genders.Find(id);
        //    if (gender == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Genders.Remove(gender);
        //    db.SaveChanges();

        //    return Ok(gender);
        //}

        [HttpGet]
        public IHttpActionResult DeativeGender(int id)
        {
            Gender gender = db.Genders.Find(id);
            if (gender == null)
            {
                return NotFound();
            }

            db.Genders.Remove(gender);
            db.SaveChanges();

            return Ok(gender);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GenderExists(int id)
        {
            return db.Genders.Count(e => e.Id == id) > 0;
        }
    }
}