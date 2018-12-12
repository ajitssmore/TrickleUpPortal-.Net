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
    public class MasterPasswordsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/MasterPasswords
        //public IQueryable<MasterPassword> GetMasterPasswords()
        //{
        //    return db.MasterPasswords;
        //}

        [HttpGet]
        public HttpResponseMessage GetMasterPasswords()
        {
            var MasterPassword = from Password in db.MasterPasswords
                          select new { Password.Id, Password.Password, Password.Active, Password.CreatedBy, Password.CreatedOn };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { MasterPassword }, success = true, error = string.Empty });
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentMasterPasswordAPP()
        {
            MasterPassword MasterPassword = db.MasterPasswords.Where(a => a.Active == true).FirstOrDefault();
                                 //where Password.Active == true
                                 //select new { Password.Id, Password.Password, Password.Active }).FirstOrDefault();
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { MasterPassword = MasterPassword.Password } , success = true, error = string.Empty });
        }

        // GET: api/MasterPasswords/5
        [ResponseType(typeof(MasterPassword))]
        public IHttpActionResult GetMasterPassword(int id)
        {
            MasterPassword masterPassword = db.MasterPasswords.Find(id);
            if (masterPassword == null)
            {
                return NotFound();
            }

            return Ok(masterPassword);
        }

        // PUT: api/MasterPasswords/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutMasterPassword(int id, MasterPassword masterPassword)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != masterPassword.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(masterPassword).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MasterPasswordExists(id))
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
        public HttpResponseMessage PutMasterPassword(int id, MasterPassword masterPassword)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != masterPassword.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(masterPassword).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MasterPasswordExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { masterPassword }, success = true, error = string.Empty });
        }
        // POST: api/MasterPasswords
        //[ResponseType(typeof(MasterPassword))]
        //public IHttpActionResult PostMasterPassword(MasterPassword masterPassword)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.MasterPasswords.Add(masterPassword);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = masterPassword.Id }, masterPassword);
        //}

        [HttpPost]
        public HttpResponseMessage PostMasterPassword(MasterPassword masterPassword)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.MasterPasswords.Add(masterPassword);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = masterPassword.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/MasterPasswords/5
        [ResponseType(typeof(MasterPassword))]
        public IHttpActionResult DeleteMasterPassword(int id)
        {
            MasterPassword masterPassword = db.MasterPasswords.Find(id);
            if (masterPassword == null)
            {
                return NotFound();
            }

            db.MasterPasswords.Remove(masterPassword);
            db.SaveChanges();

            return Ok(masterPassword);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MasterPasswordExists(int id)
        {
            return db.MasterPasswords.Count(e => e.Id == id) > 0;
        }
    }
}