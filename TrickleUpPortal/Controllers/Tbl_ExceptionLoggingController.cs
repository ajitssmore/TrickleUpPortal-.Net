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
    public class Tbl_ExceptionLoggingController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Tbl_ExceptionLogging
        //public IQueryable<Tbl_ExceptionLogging> GetTbl_ExceptionLogging()
        //{
        //    return db.Tbl_ExceptionLogging;
        //}
        [HttpGet]
        public HttpResponseMessage GetTbl_ExceptionLogging()
        {
            var Tbl_ExceptionLog = from Tbl_ExceptionLoggs in db.Tbl_ExceptionLogging
                    select new { Tbl_ExceptionLoggs.Logid, Tbl_ExceptionLoggs.ExceptionType, Tbl_ExceptionLoggs.ExceptionMsg, Tbl_ExceptionLoggs.Logdate };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Tbl_ExceptionLog }, success = true, error = string.Empty });
        }

        // GET: api/Tbl_ExceptionLogging/5
        [ResponseType(typeof(Tbl_ExceptionLogging))]
        public IHttpActionResult GetTbl_ExceptionLogging(long id)
        {
            Tbl_ExceptionLogging tbl_ExceptionLogging = db.Tbl_ExceptionLogging.Find(id);
            if (tbl_ExceptionLogging == null)
            {
                return NotFound();
            }

            return Ok(tbl_ExceptionLogging);
        }

        // PUT: api/Tbl_ExceptionLogging/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTbl_ExceptionLogging(long id, Tbl_ExceptionLogging tbl_ExceptionLogging)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_ExceptionLogging.Logid)
            {
                return BadRequest();
            }

            db.Entry(tbl_ExceptionLogging).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_ExceptionLoggingExists(id))
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

        // POST: api/Tbl_ExceptionLogging
        //[ResponseType(typeof(Tbl_ExceptionLogging))]
        //public IHttpActionResult PostTbl_ExceptionLogging(Tbl_ExceptionLogging tbl_ExceptionLogging)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Tbl_ExceptionLogging.Add(tbl_ExceptionLogging);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tbl_ExceptionLogging.Logid }, tbl_ExceptionLogging);
        //}

        [HttpPost]
        public HttpResponseMessage PostTbl_ExceptionLogging(Tbl_ExceptionLogging tbl_ExceptionLogging)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Tbl_ExceptionLogging.Add(tbl_ExceptionLogging);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = tbl_ExceptionLogging.Logid }, success = true, error = string.Empty });
        }

        public long SaveTbl_ExceptionLogging(Tbl_ExceptionLogging tbl_ExceptionLogging)
        {
            db.Tbl_ExceptionLogging.Add(tbl_ExceptionLogging);
            db.SaveChanges();

            return tbl_ExceptionLogging.Logid;
        }

        // DELETE: api/Tbl_ExceptionLogging/5
        [ResponseType(typeof(Tbl_ExceptionLogging))]
        public IHttpActionResult DeleteTbl_ExceptionLogging(long id)
        {
            Tbl_ExceptionLogging tbl_ExceptionLogging = db.Tbl_ExceptionLogging.Find(id);
            if (tbl_ExceptionLogging == null)
            {
                return NotFound();
            }

            db.Tbl_ExceptionLogging.Remove(tbl_ExceptionLogging);
            db.SaveChanges();

            return Ok(tbl_ExceptionLogging);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_ExceptionLoggingExists(long id)
        {
            return db.Tbl_ExceptionLogging.Count(e => e.Logid == id) > 0;
        }
    }
}