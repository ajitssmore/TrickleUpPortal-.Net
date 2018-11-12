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
    public class BulkUploadRefsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/BulkUploadRefs
        //public IQueryable<BulkUploadRef> GetBulkUploadRefs()
        //{
        //    return db.BulkUploadRefs;
        //}

        [HttpGet]
        public HttpResponseMessage GetBulkUploadRefs()
        {
            var BulkUpload = from BulkUploadRef in db.BulkUploadRefs
                             join User in db.Users on BulkUploadRef.CreatedBy equals User.Id
                             select new { BulkUploadRef.Id, BulkUploadRef.BulkUploadId, BulkUploadRef.CreatedOn, User.Name };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { BulkUpload }, success = true, error = string.Empty });
        }

        // GET: api/BulkUploadRefs/5
        [ResponseType(typeof(BulkUploadRef))]
        public IHttpActionResult GetBulkUploadRef(int id)
        {
            BulkUploadRef bulkUploadRef = db.BulkUploadRefs.Find(id);
            if (bulkUploadRef == null)
            {
                return NotFound();
            }

            return Ok(bulkUploadRef);
        }

        // PUT: api/BulkUploadRefs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBulkUploadRef(int id, BulkUploadRef bulkUploadRef)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bulkUploadRef.Id)
            {
                return BadRequest();
            }

            db.Entry(bulkUploadRef).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BulkUploadRefExists(id))
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

        // POST: api/BulkUploadRefs
        [ResponseType(typeof(BulkUploadRef))]
        public IHttpActionResult PostBulkUploadRef(BulkUploadRef bulkUploadRef)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BulkUploadRefs.Add(bulkUploadRef);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bulkUploadRef.Id }, bulkUploadRef);
        }

        // DELETE: api/BulkUploadRefs/5
        [ResponseType(typeof(BulkUploadRef))]
        public IHttpActionResult DeleteBulkUploadRef(int id)
        {
            BulkUploadRef bulkUploadRef = db.BulkUploadRefs.Find(id);
            if (bulkUploadRef == null)
            {
                return NotFound();
            }

            db.BulkUploadRefs.Remove(bulkUploadRef);
            db.SaveChanges();

            return Ok(bulkUploadRef);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BulkUploadRefExists(int id)
        {
            return db.BulkUploadRefs.Count(e => e.Id == id) > 0;
        }
    }
}